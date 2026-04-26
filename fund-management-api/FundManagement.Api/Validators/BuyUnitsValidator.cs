using FluentValidation;
using FundManagement.Api.Data;
using FundManagement.Api.DTOs.Portfolio;
using Microsoft.EntityFrameworkCore;

namespace FundManagement.Api.Validators
{
    public class BuyUnitsValidator : AbstractValidator<BuyUnitsDto>
    {
        public BuyUnitsValidator(AppDbContext context)
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .MustAsync(async (id, ct) => await context.Funds.AnyAsync(f => f.Id == id, ct))
                .WithMessage("The specified Fund does not exist.");

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .MustAsync(async (id, ct) => await context.Users.AnyAsync(u => u.Id == id, ct))
                .WithMessage("The specified User does not exist.");
        }
    }
}
