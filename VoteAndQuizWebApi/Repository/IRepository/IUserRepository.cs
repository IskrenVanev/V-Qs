using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User User);
         bool Save();
    }
}
