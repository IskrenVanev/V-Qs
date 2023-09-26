using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Dto;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuizzesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly ILogger _logger;
         private readonly IQuizRepository _quizRepository;
        public QuizzesController(IUnitOfWork unitOfWork , IQuizRepository quizRepository)
        {
            _unitOfWork = unitOfWork;
             _quizRepository = quizRepository;
            //  _logger = logger;
        }
        [HttpGet(Name = "Index")]
        public IActionResult Index()//Lists all quizzes on the main quiz page
        {
            IQueryable<Quiz> quizzesQuery = _unitOfWork.Quiz.GetAll()
                .Include(q => q.Creator)
                .Include(q => q.Options)
                .Include(q => q.CorrectOption);

            // Project the Quiz objects into QuizViewModel objects
            var quizzesList = quizzesQuery.Select(q => new QuizDTO
            {
                Id = q.Id,
                Name = q.Name,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
                QuizEndDate = q.QuizEndDate,
                quizVotes = q.quizVotes,
                Options = q.Options,
                IsActive = q.IsActive,
                IsDeleted = q.IsDeleted,
                ShowQuiz = q.ShowQuiz,

            }).ToList();

            return Json(quizzesList);


        }
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public IActionResult Details(int? id)//You should be able to access Details page to vote for a quiz option.
        {
            if (id == null)
            {
                return BadRequest();
            }


            var quiz = _unitOfWork.Quiz.Get(q => q.Id == id, "Options");



            if (quiz == null)
            {
                return NotFound();
            }

            var quizVm = new QuizDTO
            {
                Id = quiz.Id,
                Name = quiz.Name,
                CreatedAt = quiz.CreatedAt,
                UpdatedAt = quiz.UpdatedAt,
                QuizEndDate = quiz.QuizEndDate,
                quizVotes = quiz.quizVotes,
                Options = quiz.Options,
                IsActive = quiz.IsActive,
                IsDeleted = quiz.IsDeleted,
                ShowQuiz = quiz.ShowQuiz,
            };



            return Json(quizVm);
        }
        [HttpPost]
        public  IActionResult Create([FromBody] Quiz obj)//this is the post method for creating a new quiz
        {
            if (!_quizRepository.CreateQuiz(obj))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }


            return Ok(obj);
        }


    }
}
