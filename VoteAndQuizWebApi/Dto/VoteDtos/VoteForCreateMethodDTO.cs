using Newtonsoft.Json;
using VoteAndQuizWebApi.Dto.VoteDtos;
using VoteAndQuizWebApi.Dto;
using System.ComponentModel.DataAnnotations;

namespace VoteAndQuizWebApi.Dto.VoteDtos
{
    public class VoteForCreateMethodDTO
    {
        public string Name { get; set; }
    //    [JsonIgnore]
    //    public string CreatorId { get; set; }
        public DateTime VoteEndDate { get; set; }
        [MinLength(2, ErrorMessage = "At least two options are required.")]
        public ICollection<VoteOptionDTO> Options { get; set; }
            
        public VoteForCreateMethodDTO()
        {
            VoteEndDate = DateTime.UtcNow.AddHours(3); // Set a default date
            Options = new List<VoteOptionDTO>();
        }
    }
}
// public class Vote
// {
//     [Key]
//     public int Id { get; set; }
//     public Guid CreatorId { get; set; }
//     [ForeignKey("CreatorId")]
//     public User Creator { get; set; }
//     [Required]
//     public string Name { get; set; }
//
//     public DateTime? UpdatedAt { get; set; }
//     public DateTime? DeletedAt { get; set; }
//     public DateTime CreatedAt { get; set; }
//     [Required]
//     public DateTime VoteEndDate { get; set; }//date of ending the vote
//
//
//     public ICollection<VoteOption> Options { get; set; }
//
//     public long voteVotes { get; set; }
//     [Required]
//     public bool IsActive { get; set; }
//     [Required]
//     public bool IsDeleted { get; set; }
//     [Required]
//     public bool ShowVote { get; set; }
// }


//public class VoteForCreateMethodDTO
//{


//    public string Name { get; set; }

//    public UserDTO Creator { get; set; } // the user shouldnt be able to set this. it should automatically be set



//    public DateTime? UpdatedAt { get; private set; }// the user shouldnt be able to set this. it should automatically be set

//    public DateTime CreatedAt { get; private set; }// the user shouldnt be able to set this. it should automatically be set

//    public DateTime VoteEndDate { get; set; }//date of ending the vote


//    public ICollection<VoteOptionDTO> Options { get; set; }

//    public long voteVotes { get; private set; }

//    public bool IsActive { get; private set; } // the user shouldnt be able to set this. it should automatically be set

//    public bool IsDeleted { get; private set; } // the user shouldnt be able to set this. it should automatically be set

//    public bool ShowVote { get; private set; } // the user shouldnt be able to set this. it should automatically be set



//    public VoteForCreateMethodDTO()
//    {

//        voteVotes = 0;
//        UpdatedAt = DateTime.UtcNow.AddHours(3); // or set it to the default DateTime
//        CreatedAt = DateTime.UtcNow.AddHours(3); // or set it to the default DateTime
//        voteVotes = 0; // or set it to the default value
//        IsActive = true; // or set it to the default value
//        IsDeleted = false; // or set it to the default value
//        ShowVote = true; // or set it to the default value
//    }
//}