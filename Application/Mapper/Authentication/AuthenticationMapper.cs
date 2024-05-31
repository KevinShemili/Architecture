using Application.UseCases.Authentication.Commands;
using AutoMapper;
using Domain.Entities.IdentityExtensions;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Mapper.Authentication
{
    public class AuthenticationMapper : Profile
    {
        public AuthenticationMapper()
        {
            CreateMap<RegisterRequest, RegisterCommand>();

            CreateMap<RegisterCommand, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
