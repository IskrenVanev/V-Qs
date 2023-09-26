using System.ComponentModel.DataAnnotations;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Dto
{
    public class VoteDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime VoteEndDate { get; set; }//date of ending the vote


        public ICollection<VoteOption> Options { get; set; }

        public long voteVotes { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public bool ShowVote { get; set; }
    }
}
