using System.ComponentModel.DataAnnotations;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Dto.VoteDtos
{
    public class VoteDTO
    {
        
        public int Id { get; set; }

      
        public string Name { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime VoteEndDate { get; set; }//date of ending the vote


        public ICollection<VoteOptionDTO> Options { get; set; }

        public long voteVotes { get; set; }
        
        public bool IsActive { get; set; }
     
        public bool IsDeleted { get; set; }
        
        public bool ShowVote { get; set; }
    }
}
