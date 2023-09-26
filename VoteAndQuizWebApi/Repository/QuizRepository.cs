using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Repository
{
    public class QuizRepository : Repository<Quiz>, IQuizRepository
    {
        private ApplicationDbContext _db;
        public QuizRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Quiz obj)
        {
            _db.Quizzes.Update(obj);
        }
        public bool CreateQuiz( Quiz quiz)
        {
            try
            {
               //  Check if the quiz creator exists
                var creator =  _db.Users.FirstOrDefaultAsync(u => u.Id == quiz.CreatorId);
                if (creator == null)
                {

                    return false;
                }

                // Create a new quiz
                var newQuiz = new Quiz          //TODO:FIX THIS BECAUSE IT IS NULL
                {
                    CreatorId = quiz.CreatorId,
                    Creator = quiz.Creator,
                    Name = quiz.Name,
                    CreatedAt = DateTime.Now,
                    QuizEndDate = quiz.QuizEndDate,
                    Options = quiz.Options,
                    CorrectOptionId = quiz.CorrectOptionId,
                    CorrectOption = quiz.CorrectOption,
                    IsActive = quiz.IsActive = true,
                    IsDeleted = quiz.IsDeleted = false,
                    UpdatedAt = null,
                    DeletedAt = null,
                    ShowQuiz = true,
                    quizVotes = 0 // Initialize vote count to 0

                };


                _db.Quizzes.Add(newQuiz); 
                _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

               // _logger.LogError(ex, "An error occurred while creating a quiz.");
                return false;
            }
        }

    }
}
