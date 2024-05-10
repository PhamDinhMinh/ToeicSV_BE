using System.Collections;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Auth;
using Do_An_Tot_Nghiep.Dto.ExamTips;
using Do_An_Tot_Nghiep.Dto.Grammar;
using Do_An_Tot_Nghiep.Dto.Post;
using Do_An_Tot_Nghiep.Dto.PostComment;
using Do_An_Tot_Nghiep.Dto.PostReact;
using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Enums.ExamTips;
using Do_An_Tot_Nghiep.Enums.Grammar;
using Do_An_Tot_Nghiep.Models;

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
        CreateMap<UpdatePostDto, Post>()
            .ForMember(dest => dest.ContentPost,
                opt => opt.Condition(src => !string.IsNullOrEmpty(src.ContentPost)))
            .ForMember(dest => dest.BackGroundId,
                opt => opt.Condition(src => src.BackGroundId.HasValue))
            .ForMember(dest => dest.EmotionId,
                opt => opt.Condition(src => src.EmotionId.HasValue))
            .ForMember(dest => dest.State,
                opt => opt.Condition(src => src.State.HasValue))
            .ForMember(dest => dest.ImageUrls,
                opt => opt.Condition(src => src.ImageUrls != null && src.ImageUrls.Any()));
        CreateMap<CreatePostCommentDto, PostComment>().ReverseMap();
        
        CreateMap<CreatePostReactDto, PostReact>().ReverseMap();

        CreateMap<CreateQuestionSingleDto, QuestionToeic>().ReverseMap();
        CreateMap<CreateQuestionSingleDto, AnswerToeic>().ReverseMap();

        CreateMap<CreateQuestionGroupDto, GroupQuestion>().ReverseMap();
        CreateMap<CreateQuestionGroupDto, QuestionToeic>().ReverseMap();
        CreateMap<CreateQuestionGroupDto, AnswerToeic>().ReverseMap();

    }
    #region method helpers

    private static bool IsNotNullOrDefault<T>(T srcMember)
    {
        if (srcMember is IEnumerable list) return list.GetEnumerator().MoveNext();
        return srcMember != null && !EqualityComparer<T>.Default.Equals(srcMember, default);
    }

    #endregion
}