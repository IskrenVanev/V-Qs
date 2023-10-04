namespace VoteAndQuizWebApi.Dto;

public class QuizForCreateMethodDTO
{
    public UserDTO Creator { get; set; }
    public string Name { get; set; }
    public string CreatedAt { get; set; }
    public string QuizEndDate { get; set; }
    
    public ICollection<UserQuizAnswerDTO> Options { get; set; }
    
    
    
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string UpdatedAt { get; set; } = null;
    public string DeletedAt { get; set; } = null;
    public bool ShowQuiz { get; set; } = true;
    public int quizVotes { get; set; } = 0;
    
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