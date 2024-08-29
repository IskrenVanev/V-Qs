using System.ComponentModel.DataAnnotations;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Dto
{
    public class QuizDTO
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; }

       
        
        public UserDTO Creator { get; set; }
        
        public DateTime QuizEndDate { get; set; }//date of ending the quiz
        public long quizVotes { get; set; }
       
        public ICollection<UserQuizAnswerDTO> Options { get; set; }
        public WinnerQuizOptionDTO CorrectOption { get; set; }
        
        
        
        
        // public DateTime? UpdatedAt { get; set; }
        //
        // public DateTime CreatedAt { get; set; }
        // public bool IsActive { get; set; }
        // public bool IsDeleted { get; set; }
        // public bool ShowQuiz { get; set; }


    }
}
// [Key]
// public int Id { get; set; }
// [Required]
// public string Name { get; set; }
// public Guid CreatorId { get; set; }
// [ForeignKey("CreatorId")]
// [Required]
// public User Creator { get; set; }
// public DateTime? UpdatedAt { get; set; }
// public DateTime? DeletedAt { get; set; }
// public DateTime CreatedAt { get; set; }
// [Required]
// public DateTime QuizEndDate { get; set; }//date of ending the quiz
// public long quizVotes { get; set; }
//
// [Required]
// public ICollection<UserQuizAnswer> Options { get; set; }
//
// public int CorrectOptionId { get; set; }
// [Required]
// [ForeignKey("CorrectOptionId")]
//
// public QuizOption CorrectOption { get; set; }
// [Required]
// public bool IsActive { get; set; }
// [Required]
// public bool IsDeleted { get; set; }
// [Required]
// public bool ShowQuiz { get; set; }