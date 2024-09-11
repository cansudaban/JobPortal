using FluentValidation;
using JobPortal.Common.Helpers;
using JobPortal.Data.Models;

namespace JobPortal.Data.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("UserNameRequired"))
                .MaximumLength(100).WithMessage(ResourceHelper.GetMessage("UserNameMaxLength"));

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("EmailRequired"))
                .EmailAddress().WithMessage(ResourceHelper.GetMessage("InvalidEmailFormat"));

            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("PhoneNumberRequired"))
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(ResourceHelper.GetMessage("InvalidPhoneNumberFormat"));

            RuleFor(u => u.Address)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("AddressRequired"))
                .MaximumLength(200).WithMessage(ResourceHelper.GetMessage("AddressMaxLength"));
        }
    }
}
