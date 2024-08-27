using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using AutoMapper;
using VoteAndQuizWebApi.Dto;


using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace VoteAndQuizWebApi.Controllers
{
    [Route("api/Quizzes")]
    [ApiController]
    public class QuizzesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQuizRepository _quizRepository;
        private readonly UserManager<User> _userManager;
        public QuizzesController(IUnitOfWork unitOfWork, IQuizRepository quizRepository, IMapper mapper
            , UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _quizRepository = quizRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index() //Lists all quizzes on the main quiz page
        {
            var quizzes = _mapper.Map<List<QuizForIndexMethodDTO>>(_unitOfWork.Quiz.GetAll(q => !q.IsDeleted).Include(q => q.Options));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Json(quizzes);
        }

        [HttpGet("Details/{id}")]
        [ProducesResponseType(200)]
        public IActionResult Details(int? id) //You should be able to access Details page to vote for a quiz option.
        {
            if (id == null)
                return BadRequest();

            //Details Method Dto ***
            var quiz = _mapper.Map<QuizForIndexMethodDTO>(_unitOfWork.Quiz.Get(q => q.Id == id, "Options"));

            if (quiz == null)
                return NotFound();

            if (quiz.IsDeleted == true)
                return BadRequest();

            return Json(quiz);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] QuizForCreateMethodDTO quizDto)
        {
            // Validate input
            if (quizDto == null || quizDto.Options == null || quizDto.Options.Count < 2)
            {
                ModelState.AddModelError("", "Quiz must have at least 2 options.");
                return BadRequest(ModelState);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            if (user == null) return Unauthorized("Log in to create a quiz");

            // Check if QuizEndDate is less than one day from now
            if (quizDto.QuizEndDate < DateTime.UtcNow.AddDays(1))
            {
                ModelState.AddModelError("", "Quiz end date must be at least one day from today.");
                return BadRequest(ModelState);
            }

            var newQuiz = new Quiz
            {
                Name = quizDto.Name,
                UpdatedAt = DateTime.UtcNow.AddHours(3),
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow.AddHours(3),
                CreatorId = userId,
                QuizEndDate = quizDto.QuizEndDate == DateTime.MinValue ? DateTime.UtcNow.AddDays(14) : quizDto.QuizEndDate,
                quizVotes = quizDto.quizVotes,
                Options = _mapper.Map<List<UserQuizAnswer>>(quizDto.Options),
                CorrectOption = _mapper.Map<QuizOption>(quizDto.CorrectOption),
                IsActive = true,
                IsDeleted = false,
                ShowQuiz = true,

            };

            var quizObj = _quizRepository.Get(q => q.Name.Trim().ToUpper() == quizDto.Name.TrimEnd().ToUpper());
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

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            if (user == null) return Unauthorized();

            // Check if the logged-in user is the creator of the quiz
            if (quizToDelete.CreatorId != userId)
            {
                return Unauthorized("You are not authorized to delete this quiz.");
            }

            if (!_quizRepository.DeleteQuiz(quizToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting quiz");
            }

            return Ok("Successfully deleted");
        }
        [HttpPut("{quizId}")] //this does not make sense... add voting endpoint
        public IActionResult UpdateQuiz(int quizId)//updating quiz happens when someone votes for a quiz option
        {
            var quiz = _unitOfWork.Quiz.Get(q => q.Id == quizId);
            if (quiz == null)
                return BadRequest(ModelState);
            if (quiz.IsDeleted == true)
                return BadRequest(ModelState);
            quiz.UpdatedAt = DateTime.UtcNow.AddHours(3); 
            quiz.quizVotes += 1;
            _unitOfWork.Quiz.Update(quiz);
            _unitOfWork.Save();
            return Ok("Successfully updated quiz");
        }
        
        [HttpPost("Finish/{quizId}")]
        public IActionResult FinishQuiz(int quizId) //only creator can finish it
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            if (user == null) return Unauthorized("Log in to finish this quiz");

            var quiz = _unitOfWork.Quiz.Get(q => q.Id == quizId);
            if (quiz == null)
                return BadRequest(ModelState);

            if (quiz.CreatorId != userId)
            {
                return Unauthorized("You are not authorized to finish this quiz.");
            }

            quiz.QuizEndDate = DateTime.UtcNow.AddHours(3);
            quiz.IsActive = false;
            quiz.ShowQuiz = false;
            quiz.IsDeleted = true;
            quiz.UpdatedAt = DateTime.UtcNow.AddHours(3);
            quiz.DeletedAt = DateTime.UtcNow.AddHours(3);

            _unitOfWork.Quiz.Update(quiz);
            _unitOfWork.Save();

            return Ok("Successfully finished quiz");
        }
    }
}