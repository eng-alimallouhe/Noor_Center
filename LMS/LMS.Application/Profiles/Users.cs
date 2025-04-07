using AutoMapper;
using LMS.Application.DTOs.RequestDTOs;
using LMS.Application.DTOs.ResponseDTOs;
using LMS.Domain.Entities.Users;

namespace LMS.Application.Profiles
{
    public class Users: Profile
    {
        public Users()
        {
            CreateMap<Customer, RegisterRequestDTO>().ReverseMap();
            CreateMap<User, UserResponseDTO>().ReverseMap();
        }
    }
}
