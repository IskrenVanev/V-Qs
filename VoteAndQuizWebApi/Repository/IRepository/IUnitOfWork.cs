namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IQuizOptionRepository QuizOption { get; }
        IQuizRepository Quiz { get; }
        IUserQuizAnswerRepository UserQuizAnswer { get; }
        IUserRepository User { get; }
        IUserVoteAnswerRepository UserVoteAnswer { get; }
        IVoteOptionRepository VoteOption { get; }
        IVoteRepository Vote { get; }
        void Save();
    }
}
