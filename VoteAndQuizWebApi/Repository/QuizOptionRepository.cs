using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class QuizOptionRepository : Repository<QuizOption>, IQuizOptionRepository
    {
        private ApplicationDbContext _db;
        public QuizOptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(QuizOption obj)
        {
            _db.QuizOptions.Update(obj);
        }
    }
}
