namespace Central_De_Erros
{
    using AutoMapper;
    using Central_De_Erros.DTOs;
    using Central_De_Erros.Models;
    using Central_De_Erros.ViewModel;
    using Microsoft.AspNetCore.Identity;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Error, ErrorDTO>().ReverseMap();

            CreateMap<Error, ErrorViewModel>().ReverseMap();

            CreateMap<LoginUserSucceeded, IdentityUser>().ReverseMap();

            CreateMap<UserViewCreated, IdentityUser>().ReverseMap();

        }
    }
}