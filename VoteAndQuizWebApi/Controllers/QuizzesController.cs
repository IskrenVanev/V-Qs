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
            var quizzes = _mapper.Map<List<QuizForIndexMethodDTO>>(_unitOfWork.Quiz.GetAll(q => q.IsActive).Include(q => q.Options));
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
                return BadRequest("This Quiz no longer exists");

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
                CorrectOption = _mapper.Map<WinnerQuizOption>(quizDto.CorrectOption),
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
            if (user == null) return Unauthorized("Log in to delete a quiz");

            if (quizToDelete.IsDeleted)
                return BadRequest("This quiz is already deleted!");

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
        [HttpPut("{quizId}/vote/{answerId}")]
        public IActionResult VoteForQuiz(int quizId, int answerId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            if (user == null) return Unauthorized("Log in to vote for a quiz");

            // Get the quiz from the repository
            var quiz = _unitOfWork.Quiz.Get(q => q.Id == quizId);
            if (quiz == null || quiz.IsDeleted)
                return NotFound("Quiz not found or has been deleted.");

            // Check if the quiz is still active
            if (!quiz.IsActive || quiz.QuizEndDate < DateTime.UtcNow.AddHours(3))
                return BadRequest("Voting is closed for this quiz.");

            // Get the specific answer from the quiz options
            var answer = _unitOfWork.UserQuizAnswer.Get(uqa => uqa.Id == answerId);
            if (answer == null)
                return NotFound("Quiz answer not found.");

            // Increment the vote count for the selected answer
            answer.quizAnswerVotes += 1;

            // Increment the total vote count for the quiz
            quiz.quizVotes += 1;
            quiz.UpdatedAt = DateTime.UtcNow.AddHours(3);

            // Save the changes to the database
            _unitOfWork.UserQuizAnswer.Update(answer);
            _unitOfWork.Quiz.Update(quiz);
            _unitOfWork.Save();

            return Ok("Successfully voted for the quiz answer.");
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
            quiz.UpdatedAt = DateTime.UtcNow.AddHours(3);

            _unitOfWork.Quiz.Update(quiz);
            _unitOfWork.Save();

            return Ok("Successfully finished quiz");
        }

        [HttpGet("CorrectOption/{quizId}")]
        public IActionResult GetCorrectOption(int quizId)
        {
            // Fetch the quiz by ID
            var quiz = _unitOfWork.Quiz.Get(q => q.Id == quizId);
            if (quiz == null || quiz.IsDeleted)
                return NotFound("Quiz not found or has been deleted.");

            // Check if the quiz has been finished
            if (quiz.IsActive)
            {
                return BadRequest("Quiz is still active. The correct option is not available yet.");
            }

            // Retrieve the correct (winning) option
            var correctOption = _unitOfWork.WinnerQuizOption.Get(o => o.QuizId == quiz.Id);
            if (correctOption == null)
                return NotFound("Correct option not found.");

            // Return the correct option
            return Ok(correctOption);
        }
    }
}