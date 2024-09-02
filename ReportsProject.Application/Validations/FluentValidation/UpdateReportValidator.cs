using FluentValidation;
using ReportsProject.Domain.Dto;

namespace ReportsProject.Application.Validations.FluentValidation;

public class UpdateReportValidator : AbstractValidator<UpdateReportDto>
{
    public UpdateReportValidator()
    {
        RuleFor(report => report.Id).NotEmpty();
        RuleFor(report => report.Name).NotEmpty().MaximumLength(200);
        RuleFor(report => report.Description).NotEmpty().MaximumLength(1000);
    }
}
