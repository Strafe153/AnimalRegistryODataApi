using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Application.AutoMapperProfiles;

public class OwnerProfile : Profile
{
    public OwnerProfile()
    {
        CreateMap<Owner, OwnerDto>()
            .ReverseMap()
            .ForMember(o => o.Id, opt => opt.Ignore());
    }
}