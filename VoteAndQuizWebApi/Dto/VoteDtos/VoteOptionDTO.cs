using System.ComponentModel.DataAnnotations;

namespace VoteAndQuizWebApi.Dto.VoteDtos
{
    public class VoteOptionDTO
    {
        public long voteCount { get; set; }
        public string Option { get; set; }
    }
}
