using System.Collections;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Auth;
using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Models;
using Microsoft.Extensions.DependencyInjection;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserRegisterDto, User>().ReverseMap();
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.EmailAddress, 
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.EmailAddress)))
            .ForMember(dest => dest.Gender, 
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.Gender)))
            .ForMember(dest => dest.Name, 
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
            .ForMember(dest => dest.PhoneNumber, 
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.PhoneNumber)))
            .ForMember(dest => dest.ImageUrl, 
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.ImageUrl)))
            .ForMember(dest => dest.CoverImageUrl, 
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.CoverImageUrl)))
            .ForMember(dest => dest.DateOfBirth,
                opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
    }

    #region method helpers

    private static bool IsNotNullOrDefault<T>(T srcMember)
    {
        if (srcMember is IEnumerable list) return list.GetEnumerator().MoveNext();
        return srcMember != null && !EqualityComparer<T>.Default.Equals(srcMember, default);
    }

    #endregion
}