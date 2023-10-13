using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class VoteOptionRepository : Repository<VoteOption>, IVoteOptionRepository
    {
        private ApplicationDbContext _db;
        public VoteOptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(VoteOption obj)
        {
            _db.VoteOptions.Update(obj);
        }
        public void Attach(VoteOption obj)
        {
            // Attach the entity to the context
            _db.Attach(obj);
        }

        public void Detach(VoteOption entity)
        {
            _db.Entry(entity).State = EntityState.Detached;
        }
    }
}
