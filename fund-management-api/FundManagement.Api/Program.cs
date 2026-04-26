using FluentValidation;
using FluentValidation.AspNetCore;
using FundManagement.Api.Data;
using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Data.Repositories;
using FundManagement.Api.Data.Seed;
using FundManagement.Api.Middleware;
using FundManagement.Api.Services;
using FundManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Database configuration
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

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "FundManagementAPI", Version = "v1" });

    // 1. Define the Bearer Security Scheme
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter your JWT token only (without 'Bearer ' prefix)",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    // 2. Apply the Security Requirement globally
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

// Data seeding for dev environment
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.OAuthUsePkce(); // add this
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

await app.RunAsync();
