using AutoMapper;
using VoteAndQuizWebApi.Dto;
using VoteAndQuizWebApi.Dto.VoteDtos;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Helper;

public class MappingProfiles:Profile
{
    public MappingProfiles()
    {
        // Mapping for get methods
{
    CreateMap<Quiz, QuizForIndexMethodDTO>()
        .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options.Select(o => o.UserAnswer)));
    CreateMap<UserQuizAnswer, UserQuizAnswerDTO>();
}

// Mapping for create post method
{
    CreateMap<QuizDTO, QuizForCreateMethodDTO>();
    CreateMap<QuizForCreateMethodDTO, Quiz>();
    CreateMap<QuizForCreateMethodDTO, Quiz>()
        .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id)); // Assuming UserDTO has an Id property
    CreateMap<Quiz, QuizForCreateMethodDTO>()
        .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options.Select(o => o.UserAnswer)));
}

// Add mapping for UserDTO to User
{
    CreateMap<UserDTO, User>();
    CreateMap<UserQuizAnswerDTO, UserQuizAnswer>();
}

// Mapping for votes
{
    CreateMap<Vote, VoteDTO>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
        .ForMember(dest => dest.VoteEndDate, opt => opt.MapFrom(src => src.VoteEndDate))
        .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
        .ForMember(dest => dest.voteVotes, opt => opt.MapFrom(src => src.voteVotes))
        .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
        .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
        .ForMember(dest => dest.ShowVote, opt => opt.MapFrom(src => src.ShowVote));
    CreateMap<VoteOption, VoteOptionDTO>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Option, opt => opt.MapFrom(src => src.Option))
        .ForMember(dest => dest.VoteCount, opt => opt.MapFrom(src => src.VoteCount));
    
    // Reverse mapping for VoteDTO to Vote
    CreateMap<VoteDTO, Vote>();
}

// Mapping for vote create post method
{
    CreateMap<VoteForCreateMethodDTO, Vote>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
        .ForMember(dest => dest.VoteEndDate, opt => opt.MapFrom(src => src.VoteEndDate))
        .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
        .ForMember(dest => dest.voteVotes, opt => opt.MapFrom(src => src.voteVotes))
        .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
        .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
        .ForMember(dest => dest.ShowVote, opt => opt.MapFrom(src => src.ShowVote));
    CreateMap<VoteOptionDTO, VoteOption>()
        .ForMember(dest => dest.Id, opt => opt.Ignore());
    CreateMap<VoteOption, VoteOptionDTO>();
}
    }
}