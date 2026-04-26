using FluentValidation;
using FundManagement.Api.DTOs.Fund;

namespace FundManagement.Api.Validators
{
    public class UpdateFundValidator : AbstractValidator<UpdateFundDto>
    {
        private static readonly string[] AllowedCategories =
            ["Equity", "Debt", "Hybrid", "Bond", "Balanced", "Index", "Money Market"];

        public UpdateFundValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Fund name is required.")
                .MaximumLength(200).WithMessage("Fund name cannot exceed 200 characters.");

            RuleFor(x => x.NAV)
                .GreaterThan(0).WithMessage("NAV must be greater than zero.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.")
                .Must(c => AllowedCategories.Contains(c, StringComparer.OrdinalIgnoreCase))
                .WithMessage($"Category must be one of: {string.Join(", ", AllowedCategories)}.");
        }
    }
}
