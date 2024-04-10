using System.Collections;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Auth;
using Do_An_Tot_Nghiep.Dto.ExamTips;
using Do_An_Tot_Nghiep.Dto.Grammar;
using Do_An_Tot_Nghiep.Dto.Post;
using Do_An_Tot_Nghiep.Dto.PostComment;
using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Enums.ExamTips;
using Do_An_Tot_Nghiep.Enums.Grammar;
using Do_An_Tot_Nghiep.Models;
using Do_An_Tot_Nghiep.Services.Grammar;
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
        CreateMap<GrammarCreateDto, Grammar>().ReverseMap();
        CreateMap<GrammarUpdateWatchDto, Grammar>().ReverseMap();
        CreateMap<GrammarUpdateDto, Grammar>()
            .ForMember(dest => dest.Content,
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.Content)))
            .ForMember(dest => dest.Title,
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
            .ForMember(dest => dest.Type,
                opt => opt.Condition(src => src.Type.HasValue && Enum.IsDefined(typeof(EGRAMMAR_TYPE), src.Type)));
        CreateMap<ExamTipsCreateDto, ExamTip>().ReverseMap();
        CreateMap<ExamTipsUpdateDto, ExamTip>()
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description != null && src.Description.Length > 0 ? src.Description : null))
            .ForMember(dest => dest.Title,
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
            .ForMember(dest => dest.Type,
                opt => opt.Condition(src => src.Type.HasValue && Enum.IsDefined(typeof(EEXAM_TIPS_TYPE), src.Type)));
        CreateMap<CreatePostDto, Post>().ReverseMap();

        CreateMap<CreatePostCommentDto, PostComment>().ReverseMap();
    }
    #region method helpers

    private static bool IsNotNullOrDefault<T>(T srcMember)
    {
        if (srcMember is IEnumerable list) return list.GetEnumerator().MoveNext();
        return srcMember != null && !EqualityComparer<T>.Default.Equals(srcMember, default);
    }

    #endregion
}