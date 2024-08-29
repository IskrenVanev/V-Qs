using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IWinnerQuizOptionRepository : IRepository<WinnerQuizOption>
    {
        void Update(WinnerQuizOption obj);
    }
}
