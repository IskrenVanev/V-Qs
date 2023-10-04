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
        public QuizzesController(IUnitOfWork unitOfWork , IQuizRepository quizRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
             _quizRepository = quizRepository;
             _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()//Lists all quizzes on the main quiz page
        {
            var quizzes = _mapper.Map<List<QuizForIndexMethodDTO>>(_unitOfWork.Quiz.GetAll().Include(q => q.Options));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Json(quizzes);
        }
        
         [HttpGet("{id}")]
         [ProducesResponseType(200)]
         public IActionResult Details(int? id)//You should be able to access Details page to vote for a quiz option.
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
         public IActionResult Create([FromBody] QuizForCreateMethodDTO quiz)//this is the post method for creating a new quiz
         {
            
            
             if (quiz == null)
                 return BadRequest(ModelState);
             var newQuiz = _mapper.Map<Quiz>(quiz);   
             
             //var newQuiz = _mapper.Map<QuizForCreateMethodDTO>(typeof(Quiz));//quiz is dto, it is not the real model from database
             Console.WriteLine("HEREEEEEEEEEEEEEEEEEEE");
             
             
             
             if (!ModelState.IsValid)
                 return BadRequest(ModelState);
             
             
             if (!_quizRepository.CreateQuiz(newQuiz))
             {
                 ModelState.AddModelError("", "Something went wrong while saving");
                 return StatusCode(500, ModelState);
             }
             
             
             return Ok("Successfully created");
             
             
             
             // if (!string.TryParse(quiz.Creator.Id, out string creatorId))
             // {
             //     // Handle the case where quiz.Creator.Id is not a valid integer.
             //     ModelState.AddModelError("Creator.Id", "Invalid Creator ID format");
             //     return BadRequest(ModelState);
             // }
             // var TheUser = new User
             // {
             //     Id = creatorId,
             //     UserName = quiz.Creator.UserName,
             //     AuthId = quiz.Creator.AuthId,
             //     Wins = quiz.Creator.Wins,
             //     Loses = quiz.Creator.Loses,
             // };
             //
             //
             //  var newQuiz = new Quiz         
             //  {
             //     
             //      Creator = TheUser,
             //      Name = quiz.Name,
             //      CreatedAt = DateTime.Now,
             //      QuizEndDate = quiz.QuizEndDate,
             //     
             //      
             //    
             //      IsActive  = true,
             //      IsDeleted  = false,
             //      UpdatedAt = null,
             //      DeletedAt = null,
             //      ShowQuiz = true,
             //      quizVotes = 0 // Initialize vote count to 0
             //          
             //  };
        
             //var quizObj = _quizRepository.Get(q => q.Name.Trim().ToUpper() == quiz.Name.TrimEnd().ToUpper());
        
             //if (quizObj != null)
             //{
             //    ModelState.AddModelError("", "Quiz already exists");
             //    return StatusCode(422, ModelState);
             //}
        
            
        
        
             // if (!_quizRepository.CreateQuiz(newQuiz))
             // {
             //     ModelState.AddModelError("", "Something went wrong while saving");
             //     return StatusCode(500, ModelState);
             // }
        
        
             
         }

        
        
        
        
        
         
         
         
         
         
        
        


    }
}
