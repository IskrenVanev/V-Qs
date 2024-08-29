using System.ComponentModel.DataAnnotations;

namespace VoteAndQuizWebApi.Models
{
    public class WinnerQuizOption //the correct answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Answer { get; set; }

        public int QuizId { get; set; }

        public Quiz Quiz { get; set; } // Navigation property

    }
}
