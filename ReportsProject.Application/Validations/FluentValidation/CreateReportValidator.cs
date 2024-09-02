using FluentValidation;
using ReportsProject.Domain.Dto;

namespace ReportsProject.Application.Validations.FluentValidation;

public class CreateReportValidator : AbstractValidator<CreateReportDto>
{
    public CreateReportValidator()
    {
        RuleFor(report => report.Name).NotEmpty().MaximumLength(200);
        RuleFor(report => report.Description).NotEmpty().MaximumLength(1000);
    }
}
