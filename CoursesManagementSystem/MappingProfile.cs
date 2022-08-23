using AutoMapper;
using CoursesManagementSystem.DTO;
using CoursesManagementSystem.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CoursesManagementSystem
{
    public class MappingProfile :Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserForRegisterDTO, User>();
            CreateMap<UserForAuthenticationDTO, User>();
        }
    }
}
