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
           
            _db.Votes.Update(obj);
        }
        public void Attach(Vote entity)
        {
            _db.Attach(entity);
            
        }
        public void Modify(Vote entity)
        {
            _db.Entry(entity).State = EntityState.Modified;

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

        public List<VoteOption> GetVoteResult(int voteId)
        {
            var vote = _db.Votes.Include(v => v.Options).FirstOrDefault(v => v.Id == voteId);
            if (vote == null)
                return new List<VoteOption>(); // Returning an empty list to indicate no result

            // Find the maximum vote count
            var maxVoteCount = vote.Options.Max(o => o.VoteCount);

            // Find all options that have the maximum vote count
            var result = vote.Options.Where(o => o.VoteCount == maxVoteCount).ToList();

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
