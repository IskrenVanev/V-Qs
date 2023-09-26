using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class UserVoteAnswerRepository : Repository<UserVoteAnswer>, IUserVoteAnswerRepository
    {
        private ApplicationDbContext _db;
        public UserVoteAnswerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(UserVoteAnswer obj)
        {
            _db.UserVoteAnswers.Update(obj);
        }
    }
}
