﻿using AutoMapper;
using Forum.Models.UserManagement;

namespace Forum.Services.UserManagement
{
    public class UserManagementMappingProfile : Profile
    {
        public UserManagementMappingProfile()
        {
            CreateMap<RegisterUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
