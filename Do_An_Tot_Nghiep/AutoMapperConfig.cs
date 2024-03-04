using AutoMapper;
using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Models;
using Microsoft.Extensions.DependencyInjection;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserRegisterDto, User>().ReverseMap();
    }
}