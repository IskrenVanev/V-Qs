using AutoMapper;
using VoteAndQuizWebApi.Dto;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Helper;

public class MappingProfiles:Profile
{
    public MappingProfiles()
    {
        // Mapping for get methods
        CreateMap<Quiz, QuizForIndexMethodDTO>()
            .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options.Select(o => o.UserAnswer)));
        CreateMap<UserQuizAnswer, UserQuizAnswerDTO>();

// Mapping for create post method
        CreateMap<QuizDTO, QuizForCreateMethodDTO>();
        CreateMap<QuizForCreateMethodDTO, Quiz>();
        CreateMap<QuizForCreateMethodDTO, Quiz>()
            .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id)); // Assuming UserDTO has an Id property
        CreateMap<Quiz, QuizForCreateMethodDTO>().ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options.Select(o => o.UserAnswer)));

// Add mapping for UserDTO to User
        CreateMap<UserDTO, User>();
        CreateMap<UserQuizAnswerDTO, UserQuizAnswer>();
    }
}