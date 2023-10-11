using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IVoteRepository : IRepository<Vote>
    {
        void Update(Vote obj);

        public bool Save();
        public bool CreateVote(Vote vote);
    }
}
