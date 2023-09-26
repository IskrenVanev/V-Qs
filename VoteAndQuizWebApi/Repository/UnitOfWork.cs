using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public IQuizOptionRepository QuizOption { get; private set; }

        public IQuizRepository Quiz { get; private set; }

        public IUserQuizAnswerRepository UserQuizAnswer { get; private set; }

        public IUserRepository User { get; private set; }

        public IUserVoteAnswerRepository UserVoteAnswer { get; private set; }

        public IVoteOptionRepository VoteOption { get; private set; }

        public IVoteRepository Vote { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            Quiz = new QuizRepository(_db);
            Vote = new VoteRepository(_db);
            QuizOption = new QuizOptionRepository(_db);
            VoteOption = new VoteOptionRepository(_db);
            UserVoteAnswer = new UserVoteAnswerRepository(_db);
            UserQuizAnswer = new UserQuizAnswerRepository(_db);



        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
