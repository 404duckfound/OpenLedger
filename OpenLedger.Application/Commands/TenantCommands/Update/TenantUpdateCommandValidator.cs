using FluentValidation;
using System.Text.RegularExpressions;

namespace OpenLedger.Application.Commands.TenantCommands.Update
{
    public class TenantUpdateCommandValidator : AbstractValidator<TenantUpdateCommand>
    {
        private readonly Regex PhoneNumberRegex = new(@"^\+?[1-9]\d*$", RegexOptions.Compiled);
        public TenantUpdateCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tenant name is required.")
                .MaximumLength(50).WithMessage("Tenant name must not exceed 50 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.TaxNumber)
                .MaximumLength(50).WithMessage("Tax number must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.TaxNumber));

            RuleFor(x => x.TaxOffice)
                .MaximumLength(50).WithMessage("Tax office must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.TaxOffice));

            RuleFor(x => x.PhoneNumber)
                .Matches(PhoneNumberRegex).WithMessage("Invalid phone number format.")
                .MaximumLength(50).WithMessage("Phone number must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Address)
                .MaximumLength(100).WithMessage("Address must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}
