using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VoteAndQuizWebApi.Models
{
    public class VoteOption
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Option { get; set; }


       
        public long VoteCount { get; set; }
        
        public int VoteId { get; set; }
    
        public Vote Vote { get; set; }
    }
}
