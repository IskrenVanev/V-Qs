using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VoteAndQuizWebApi.Models
{
    public class UserQuizAnswer
    {
        [Key]
        public int Id { get; set; }
        public string UserAnswer { get; set; }
        [Required]
        public int QuizId { get; set; } // Foreign key
        [ForeignKey("QuizId")]
        [JsonIgnore]
        public Quiz Quiz { get; set; } // Navigation property

        public long quizAnswerVotes { get; set; }//number of votes for a quiz answer
        // Other properties related to the Option

    }
}
