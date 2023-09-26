using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class UserQuizAnswerRepository : Repository<UserQuizAnswer>, IUserQuizAnswerRepository
    {
        private ApplicationDbContext _db;
        public UserQuizAnswerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(UserQuizAnswer obj)
        {
            _db.UserQuizAnswers.Update(obj);
        }
    }
}
