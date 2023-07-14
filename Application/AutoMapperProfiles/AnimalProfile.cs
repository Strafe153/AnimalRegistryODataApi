using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Application.AutoMapperProfiles;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<Animal, AnimalDto>()
            .ForMember(o => o.OwnerId, opt => opt.MapFrom(o => o.Owner.Id))
            .ReverseMap()
            .ForMember(o => o.Id, opt => opt.Ignore());
    }
}
