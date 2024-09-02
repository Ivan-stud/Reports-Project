using AutoMapper;
using ReportsProject.Domain.Dto.User;
using ReportsProject.Domain.Models;

namespace ReportsProject.Application.Mappings;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>().ReverseMap(); 
    }
}
