using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IVoteRepository : IRepository<Vote>
    {
        void Update(Vote obj);

        public bool Save();
        public bool CreateVote(Vote vote);
        public VoteOption GetVoteResult(int voteId);
        public bool VoteExists(int? id);
        public bool DeleteVote(Vote vote);
        public bool FinishVote(int? voteId);
        public bool UpdateVote(int voteId, VoteOption voteOption);
    }
}
