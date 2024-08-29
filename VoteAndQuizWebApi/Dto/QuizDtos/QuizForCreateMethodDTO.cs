using System.ComponentModel.DataAnnotations;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Dto;

public class QuizForCreateMethodDTO
{
    public string Name { get; set; }
    public DateTime? CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    [MinLength(2, ErrorMessage = "At least two options are required.")]
    public ICollection<UserQuizAnswerDTO> Options { get; set; }
    public DateTime QuizEndDate { get; set; }
    public WinnerQuizOptionDTO CorrectOption { get; set; }
    public int quizVotes { get; private set; } = 0;
    public bool ShowQuiz { get; private set; } = true;
    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; private set; } = false;
    public QuizForCreateMethodDTO()
    {
        UpdatedAt = DateTime.UtcNow.AddHours(3); 
        CreatedAt = DateTime.UtcNow.AddHours(3);
        QuizEndDate = DateTime.UtcNow.AddDays(14);
    }
}

// Creator = TheUser,
// Name = quiz.Name,
// CreatedAt = DateTime.Now,
// QuizEndDate = quiz.QuizEndDate,
//  
// IsActive  = true,
// IsDeleted  = false,
// UpdatedAt = null,
// DeletedAt = null,
// ShowQuiz = true,
// quizVotes = 0 // Initialize vote count to 0