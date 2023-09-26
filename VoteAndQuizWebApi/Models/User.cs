using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VoteAndQuizWebApi.Models
{
    public class User : IdentityUser
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string AuthId { get; set; }
        public ICollection<UserQuizAnswer> UserQuizAnswers { get; set; }
        public ICollection<UserVoteAnswer> UserVoteAnswers { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }

    }
}
