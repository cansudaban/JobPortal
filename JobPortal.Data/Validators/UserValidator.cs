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
                .NotNull().WithMessage(ResourceHelper.GetMessage("UserNameRequired"))
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("UserNameRequired"))
                .MaximumLength(100).WithMessage(ResourceHelper.GetMessage("UserNameMaxLength"));

            RuleFor(u => u.Email)
                .NotNull().WithMessage(ResourceHelper.GetMessage("EmailRequired"))
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("EmailRequired"))
                .EmailAddress().WithMessage(ResourceHelper.GetMessage("InvalidEmailFormat"));

            RuleFor(u => u.PhoneNumber)
                .NotNull().WithMessage(ResourceHelper.GetMessage("PhoneNumberRequired"))
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("PhoneNumberRequired"))
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(ResourceHelper.GetMessage("InvalidPhoneNumberFormat"));

            RuleFor(u => u.Address)
                .NotNull().WithMessage(ResourceHelper.GetMessage("AddressRequired"))
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("AddressRequired"))
                .MaximumLength(200).WithMessage(ResourceHelper.GetMessage("AddressMaxLength"));
        }
    }
}
