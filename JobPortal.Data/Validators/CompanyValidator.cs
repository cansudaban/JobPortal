using FluentValidation;
using JobPortal.Common.Helpers;
using JobPortal.Data.Models;

namespace JobPortal.Data
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("CompanyNameRequired"))
                .MaximumLength(100).WithMessage(ResourceHelper.GetMessage("CompanyNameMaxLength"));

            RuleFor(c => c.PhoneNumber)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("PhoneNumberRequired"))
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(ResourceHelper.GetMessage("InvalidPhoneNumberFormat"));

            RuleFor(c => c.Address)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("AddressRequired"));

            RuleFor(c => c.JobPostingLimit)
                .GreaterThan(0).WithMessage(ResourceHelper.GetMessage("JobPostingLimitGreaterThan"));
        }
    }
}
