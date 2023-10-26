using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoteAndQuizWebApi.Models
{
    public class UserVoteAnswer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Option { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]

        public User User { get; set; }
        public int VoteId { get; set; }
        [ForeignKey("VoteId")]

        public Vote Vote { get; set; }


    }
}
