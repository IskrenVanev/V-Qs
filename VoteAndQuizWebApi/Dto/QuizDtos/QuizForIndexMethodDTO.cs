using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Dto;

public class QuizForIndexMethodDTO
{
    
    public int Id { get; set; }
   
    public string Name { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    
    public string QuizEndDate { get; set; }//date of ending the quiz
    public int quizVotes { get; set; }
    public ICollection<string> Options { get; set; }
    public bool IsActive { get; set; }
   
    public bool IsDeleted { get; set; }
    
    public bool ShowQuiz { get; set; }
    
    
   
    
    
   

  
   

    
    
   
}