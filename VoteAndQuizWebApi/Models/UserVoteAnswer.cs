using System.ComponentModel.DataAnnotations;

namespace VoteAndQuizWebApi.Models
{
    public class UserVoteAnswer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Option { get; set; }


    }
}
