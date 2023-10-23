using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        private ApplicationDbContext _db;
        public VoteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Vote obj)
        {
            //_db.Entry(obj).State = EntityState.Detached;
            _db.Votes.Update(obj);
        }
        public void Detach(Vote entity)
        {
            _db.Entry(entity).State = EntityState.Detached;
        }
        public bool CreateVote(Vote vote)
        {
            _db.Add(vote);
            return Save();
        }

        public VoteOption GetVoteResult(int voteId)
        {
            var vote = _db.Votes.Include(v => v.Options).FirstOrDefault(v => v.Id == voteId);
            if (vote == null) return null; // think about a better way to do this
           
            var result = vote.Options.MaxBy(o => o.VoteCount);
            return result;
        }

        public bool VoteExists(int? id)
        {
            return _db.Votes.Any(p => p.Id == id);
        }

        public bool DeleteVote(Vote vote)
        {
            vote.IsDeleted = true;
            vote.DeletedAt = DateTime.UtcNow.AddHours(3);
            vote.IsActive = false;
            vote.ShowVote = false;
            Update(vote);
            return Save();
        }

        public bool FinishVote(int? voteId)
        {
           var vote = _db.Votes.Include(v => v.Options).FirstOrDefault(v => v.Id == voteId);
           if (vote == null)
           {
               return false;
           }
          
           vote.VoteEndDate = DateTime.UtcNow.AddHours(3);
           vote.IsActive = false;
           vote.UpdatedAt = DateTime.UtcNow.AddHours(3);
           vote.ShowVote = false;
           vote.DeletedAt = DateTime.UtcNow.AddHours(3);
           vote.IsDeleted = true;
           _db.Votes.Update(vote);
           _db.SaveChanges();
           
           return true;

        }
        
       
        
        
        

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }
        
    }
}
