using FluentValidation;
using FluentValidation.AspNetCore;
using FundManagement.Api.Data;
using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Data.Repositories;
using FundManagement.Api.Data.Seed;
using FundManagement.Api.Hubs;
using FundManagement.Api.Middleware;
using FundManagement.Api.Services;
using FundManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, config) =>
        config.ReadFrom.Configuration(context.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext());

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    var jwtSettings = builder.Configuration.GetSection("Jwt");

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowNextJs", policy =>
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials());
    });

    builder.Services.AddSignalR();
    builder.Services.AddControllers();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddOpenApi();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "FundManagementAPI", Version = "v1" });

        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter your JWT token only (without 'Bearer ' prefix)",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(doc =>
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", doc)] = []
            });
    });

    builder.Services.AddScoped<IFundRepository, FundRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
    builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
    builder.Services.AddScoped<IFundNAVHistoryRepository, FundNAVHistoryRepository>();

    builder.Services.AddScoped<ITransactionManager, EFTransactionManager>();

    builder.Services.AddScoped<IPortfolioService, PortfolioService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IFundService, FundService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (env.IsDevelopment())
        {
            await DataSeeder.SeedFundAndUserAsync(context);
            await DataSeeder.SeedNavHistoryAsync(context);
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.OAuthUsePkce();
        });
    }

    app.UseMiddleware<GlobalExceptionMiddleware>(); // 1. outermost — catches all exceptions
    app.UseHttpsRedirection();                      // 2. enforce TLS
    app.UseCors("AllowNextJs");                     // 3. before auth so OPTIONS preflight passes
    app.UseMiddleware<SecurityHeadersMiddleware>(); // 4. security headers on every response
    app.UseSerilogRequestLogging();                 // 5. structured HTTP request log entry
    app.UseAuthentication();                        // 6. establish identity from JWT
    app.UseAuthorization();                         // 7. enforce [Authorize] policies
    app.MapControllers();                           // 8. route to controller actions
    app.MapHub<NAVHub>("/hubs/nav");               // 9. SignalR hub

    await app.RunAsync();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
