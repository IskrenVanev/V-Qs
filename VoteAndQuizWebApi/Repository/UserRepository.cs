using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(User user)
        {
            _db.Users.Update(user);
        }
        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
