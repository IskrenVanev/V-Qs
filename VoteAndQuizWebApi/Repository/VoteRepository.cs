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
    }
}
