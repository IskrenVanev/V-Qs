using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IVoteRepository : IRepository<Vote>
    {
        void Update(Vote obj);

        public bool Save();
        public bool CreateVote(Vote vote);
        public List<VoteOption> GetVoteResult(int voteId);
        public bool VoteExists(int? id);
        public bool DeleteVote(Vote vote);
        public bool FinishVote(int? voteId);
        public void Attach(Vote entity);
        public void Modify(Vote entity);


    }
}
