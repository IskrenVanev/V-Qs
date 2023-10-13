using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using AutoMapper;
using VoteAndQuizWebApi.Dto;


using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Controllers
{
    [Route("api/Quizzes")]
    [ApiController]
    public class QuizzesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQuizRepository _quizRepository;
        public QuizzesController(IUnitOfWork unitOfWork, IQuizRepository quizRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _quizRepository = quizRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index() //Lists all quizzes on the main quiz page
        {
            var quizzes = _mapper.Map<List<QuizForIndexMethodDTO>>(_unitOfWork.Quiz.GetAll().Include(q => q.Options));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Json(quizzes);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public IActionResult Details(int? id) //You should be able to access Details page to vote for a quiz option.
        {
            if (id == null)
                return BadRequest();


            //Details Method Dto ***
            var quiz = _mapper.Map<QuizForIndexMethodDTO>(_unitOfWork.Quiz.Get(q => q.Id == id, "Options"));

            if (quiz == null)
                return NotFound();

            return Json(quiz);
        }
        [HttpPost]
        public IActionResult
            Create([FromBody] QuizForCreateMethodDTO quiz) //TODO: fix dto if needed
        {
            if (quiz == null)
                return BadRequest(ModelState);

            var newQuiz = _mapper.Map<Quiz>(quiz);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var quizObj = _quizRepository.Get(q => q.Name.Trim().ToUpper() == quiz.Name.TrimEnd().ToUpper());
            if (quizObj != null)
            {
                ModelState.AddModelError("", "Quiz already exists");
                return StatusCode(422, ModelState);
            }

            if (!_quizRepository.CreateQuiz(newQuiz))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }


            return Ok("Successfully created");
        }
        [HttpDelete("Delete/{quizId}")]
        public IActionResult DeleteQuiz(int quizId)
        {
            if (!_quizRepository.QuizExists(quizId))
            {
                return NotFound();
            }

            var quizToDelete = _unitOfWork.Quiz.Get(q => q.Id == quizId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_quizRepository.DeleteQuiz(quizToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting quiz");
            }

            return NoContent();
        }
        [HttpPut("{quizId}")]
        public IActionResult UpdateQuiz(int quizId)//updating quiz happens when someone votes for a quiz option
        {
            var quiz = _unitOfWork.Quiz.Get(q => q.Id == quizId);
            if (quiz == null)
                return BadRequest(ModelState);
            quiz.UpdatedAt = DateTime.UtcNow;
            quiz.quizVotes += 1;
            _unitOfWork.Quiz.Update(quiz);
            _unitOfWork.Save();
            return Ok("Successfully updated quiz");
        }
        
        [HttpPost("Finish/{quizId}")]
        public IActionResult FinishQuiz(int quizId)
        {
            var quiz = _unitOfWork.Quiz.Get(q => q.Id == quizId);
            if (quiz == null)
                return BadRequest(ModelState);
            quiz.QuizEndDate = DateTime.UtcNow;
            quiz.IsActive = false;
            quiz.ShowQuiz = false;
            quiz.IsDeleted = true;
            quiz.UpdatedAt = DateTime.UtcNow;
            quiz.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Quiz.Update(quiz);
            _unitOfWork.Save();

            return Ok("Successfully finished quiz");
        }
    }
}
