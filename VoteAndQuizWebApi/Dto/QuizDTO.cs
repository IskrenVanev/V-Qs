using System.ComponentModel.DataAnnotations;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Dto
{
    public class QuizDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime QuizEndDate { get; set; }//date of ending the quiz
        public long quizVotes { get; set; }
        [Required]
        public ICollection<UserQuizAnswer> Options { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool ShowQuiz { get; set; }


    }
}
