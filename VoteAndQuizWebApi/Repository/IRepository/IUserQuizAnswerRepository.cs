using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IUserQuizAnswerRepository : IRepository<UserQuizAnswer>
    {
        void Update(UserQuizAnswer obj);
    }
}
