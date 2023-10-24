using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IVoteOptionRepository : IRepository<VoteOption>
    {
        void Update(VoteOption obj);
        
        void Detach(VoteOption entity);
        //void DeleteOtions()
        public void Attach(VoteOption entity);
        public void Modify(VoteOption entity);
        public bool Save();
    }
}
