﻿using AutoMapper;
using IdentityServer.Entities;
using IdentityService.DTOs;

namespace IdentityService.Mapper
{
    public class IdentityProfile: Profile
    {
        public IdentityProfile()
        {
            CreateMap<User,NewUserDto>().ReverseMap();
            
        }
    }
}
