using FluentValidation;
using JobPortal.Common.Helpers;
using JobPortal.Data.Models;

namespace JobPortal.Data.Validators
{
    public class JobValidator : AbstractValidator<Job>
    {
        public JobValidator()
        {
            RuleFor(j => j.Position)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("PositionRequired"))
                .MaximumLength(100).WithMessage(ResourceHelper.GetMessage("PositionMaxLength"));

            RuleFor(j => j.Description)
                .NotEmpty().WithMessage(ResourceHelper.GetMessage("DescriptionRequired"))
                .MinimumLength(10).WithMessage(ResourceHelper.GetMessage("DescriptionMinLength"))
                .MaximumLength(1000).WithMessage(ResourceHelper.GetMessage("DescriptionMaxLength"));

            RuleFor(j => j.ExpirationDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage(ResourceHelper.GetMessage("ExpirationDateFuture"));

            RuleFor(j => j.QualityScore)
                .InclusiveBetween(0, 5).WithMessage(ResourceHelper.GetMessage("QualityScoreRange"));

            RuleFor(j => j.Benefits)
                .MaximumLength(200).WithMessage(ResourceHelper.GetMessage("BenefitsMaxLength"))
                .When(j => !string.IsNullOrEmpty(j.Benefits));

            RuleFor(j => j.EmploymentType)
                .MaximumLength(50).WithMessage(ResourceHelper.GetMessage("EmploymentTypeMaxLength"))
                .When(j => !string.IsNullOrEmpty(j.EmploymentType));
        }
    }
}
