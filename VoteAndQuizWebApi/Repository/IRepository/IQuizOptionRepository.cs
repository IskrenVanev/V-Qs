using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IQuizOptionRepository : IRepository<QuizOption>
    {
        void Update(QuizOption obj);
    }
}
