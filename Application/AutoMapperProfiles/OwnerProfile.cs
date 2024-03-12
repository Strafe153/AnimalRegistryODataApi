using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

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