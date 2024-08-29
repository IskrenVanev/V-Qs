using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace VoteAndQuizWebApi.Models
{
    public class VoteOption //the vote options
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Option { get; set; }


       
        public long VoteCount { get; set; }
        
        public int VoteId { get; set; }
        [ForeignKey("VoteId")]
        public Vote Vote { get; set; }
    }
}
