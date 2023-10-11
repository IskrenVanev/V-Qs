using System.ComponentModel.DataAnnotations;

namespace VoteAndQuizWebApi.Dto.VoteDtos
{
    public class VoteOptionDTO
    {
        
        public int Id { get; set; }
        public string Option { get; set; }
        public long VoteCount { get; set; }
    }
}
