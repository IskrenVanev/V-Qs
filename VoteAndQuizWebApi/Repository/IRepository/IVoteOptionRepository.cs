using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IVoteOptionRepository : IRepository<VoteOption>
    {
        void Update(VoteOption obj);
        void Attach(VoteOption obj);
        void Detach(VoteOption entity);
        //void DeleteOtions()
    }
}
