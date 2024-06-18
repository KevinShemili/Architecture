using Application.UseCases.Authentication.Commands;
using AutoMapper;

namespace WebAPI.DataTransferObjects.Authentication
{
    public class AuthenticationMappings : Profile
    {
        public AuthenticationMappings()
        {
            CreateMap<RegisterDTO, RegisterCommand>();
            CreateMap<SignInDTO, SignInCommand>();
            CreateMap<TokensDTO, RefreshTokenCommand>();
        }
    }
}
