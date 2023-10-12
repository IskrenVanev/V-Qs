using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Dto;

public class UserDTO
{
    
    public string Id { get; set; }
    
    public string UserName { get; set; }
    
    public string AuthId { get; set; }
   // public ICollection<string> UserQuizAnswers { get; set; }
  //  public ICollection<string> UserVoteAnswers { get; set; }
    public int Wins { get; set; }
    public int Loses { get; set; }
}