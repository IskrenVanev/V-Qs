using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class WinnerQuizOptionRepository : Repository<WinnerQuizOption>, IWinnerQuizOptionRepository
    {
        private ApplicationDbContext _db;
        public WinnerQuizOptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(WinnerQuizOption obj)
        {
            _db.WinnerQuizOptions.Update(obj);
        }
    }
}
