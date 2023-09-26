using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IUserVoteAnswerRepository : IRepository<UserVoteAnswer>
    {
        void Update(UserVoteAnswer obj);
    }
}
