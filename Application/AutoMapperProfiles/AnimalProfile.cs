﻿using Application.DTOs;
using AutoMapper;
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
