using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IVoteRepository : IRepository<Vote>
    {
        void Update(Vote obj);
    }
}
