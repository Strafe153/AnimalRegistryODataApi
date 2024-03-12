using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Application.AutoMapperProfiles;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<Animal, AnimalDto>()
            .ReverseMap()
            .ForMember(o => o.Id, opt => opt.Ignore());
    }
}
