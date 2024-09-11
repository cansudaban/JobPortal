using JobPortal.Data.Models;
using AutoMapper;
using JobPortal.Common.Dtos.CompanyDtos;
using JobPortal.Common.Dtos.JobDtos;
using JobPortal.Common.Dtos.UserDtos;

namespace JobPortal.WebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
