using VoteAndQuizWebApi.Dto;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Repository.IRepository
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        void Update(Quiz obj);

        public bool CreateQuiz(Quiz quiz);
        public bool QuizExists(int quizId);
        public bool DeleteQuiz(Quiz quiz);
    }
}
