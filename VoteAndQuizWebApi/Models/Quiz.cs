using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VoteAndQuizWebApi.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        [Required]
        public User Creator { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime QuizEndDate { get; set; }
        public long quizVotes { get; set; }//number of votes for a quiz

        [Required]
        public ICollection<UserQuizAnswer> Options { get; set; }

        public int CorrectOptionId { get; set; }
        [Required]
        [ForeignKey("CorrectOptionId")]

        public WinnerQuizOption CorrectOption { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public bool ShowQuiz { get; set; }
    }

}
