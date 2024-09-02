using AutoMapper;
using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Models;

namespace ReportsProject.Application.Mappings;

public class ReportMapping : Profile
{
	public ReportMapping()
	{
		CreateMap<Report, ReportDto>()
			.ForCtorParam("Id", m => m.MapFrom(report => report.Id))
			.ForCtorParam("Name", m => m.MapFrom(report => report.Name))
			.ForCtorParam("Description", m => m.MapFrom(report => report.Description))
			.ForCtorParam("DateCreated", m => m.MapFrom(report => report.CreatedAt))
			.ReverseMap();
	}
}
