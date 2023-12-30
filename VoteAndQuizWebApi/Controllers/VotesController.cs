using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Dto;
using VoteAndQuizWebApi.Dto.VoteDtos;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Controllers
{
    [Route("api/Votes")]
    [ApiController]
    public class VotesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVoteRepository _voteRepository;
        private readonly IVoteOptionRepository _voteOptionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        public VotesController(IUnitOfWork unitOfWork, IVoteRepository voteRepository,IVoteOptionRepository voteOptionRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _voteRepository = voteRepository;
            _voteOptionRepository = voteOptionRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index() //Lists all votes on the main vote page
        {
            var votes = _mapper.Map<List<VoteDTO>>(_unitOfWork.Vote.GetAll(v => !v.IsDeleted).Include(v => v.Options));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Json(votes);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
          

            //Details Method Dto ***
            var vote = _mapper.Map<VoteDTO>(_unitOfWork.Vote.Get(q => q.Id == id, "Options"));
            if (vote.IsDeleted == true)
                return BadRequest();
            if (vote == null)
                return NotFound();

            return Json(vote);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] VoteForCreateMethodDTO vote)
        //implement some kind of authentication so that when you create vote the vote's property - creator will be populated !!
         {
            if (vote == null)
                return BadRequest(ModelState);
          
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Id == userId, "UserVoteAnswers");
            if (user == null) return Unauthorized();

            var newVote = new Vote
            {
                
                Name = vote.Name,
                UpdatedAt = DateTime.UtcNow.AddHours(3),
                DeletedAt = null,
                CreatedAt = DateTime.UtcNow.AddHours(3),
                VoteEndDate = vote.VoteEndDate,
                Options = _mapper.Map<List<VoteOption>>(vote.Options),
                UserVoteAnswers = new List<UserVoteAnswer>(),
                voteVotes = 0,
                IsActive = true,
                IsDeleted = false,
                ShowVote = true,
                CreatorId = userId



            };



            var voteObj = _voteRepository.Get(v => v.Name.Trim().ToUpper() == vote.Name.TrimEnd().ToUpper());
            if (voteObj != null)
            {
                ModelState.AddModelError("", "Vote already exists");
                return StatusCode(422, ModelState);
            }

            if (!_voteRepository.CreateVote(newVote))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //voteObj = _voteRepository.Get(v => v.Name.Trim().ToUpper() == vote.Name.TrimEnd().ToUpper());
            //voteObj.CreatorId = userId;
            //_unitOfWork.Save();

            return Ok("Successfully created");
        }
        
        [HttpGet("Result/{id}")]
        public IActionResult GetVoteResult(int? id) 
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!_voteRepository.VoteExists(id))
                return NotFound();
            
            var result =  _voteRepository.GetVoteResult(id.Value);
            if (result.Vote.IsDeleted == true)
            {
                return BadRequest();
            }
            if (result == null)
            {
                ModelState.AddModelError("", "Vote result not found."); 
                return StatusCode(404, ModelState); 
            }
            if (!ModelState.IsValid)
                return BadRequest();
            
            // Map the VoteOption to the VoteOptionDTO
            var voteOptionDto = new VoteOptionDTO
            {
                Id = result.Id,
                Option = result.Option,
                VoteCount = result.VoteCount,
                // Map other properties
            };

            return Json(voteOptionDto);
        }
        
        [HttpDelete("{id}")]
        
        public IActionResult Delete(int? id)
        {
            if (!_voteRepository.VoteExists(id))
                return NotFound();
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Vote? voteFromDb = _unitOfWork.Vote.Get(u => u.Id == id);

            if (voteFromDb == null)
            {
                return NotFound();
            }
            if (!_voteRepository.DeleteVote(voteFromDb))
            {
                ModelState.AddModelError("", "Something went wrong deleting vote");
            }
            return Ok("Successfully deleted");
        }

        
        //TODO: fix the problem with swagger in finish method in the VotesController. It does not add the wins/loses to the user that has logged in



        [HttpPost("Finish/{id}")]
        [Authorize]
        public  IActionResult Finish(int? id) 
        {
            if (id == null)
            {
                return BadRequest();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//take the id of the user who clicked on this endpoint
            var vote = _unitOfWork.Vote.Get(v => v.Id == id, "Options");
            if (userId != vote.CreatorId)//this actually works
            {
                return BadRequest("You can't finish this vote because you are not the creator!");
            }


            var finished =  _voteRepository.FinishVote(id);
            long maxVoteCount = 0;
            
            if (finished)
            {
           
                
                if (vote.Options != null && vote.Options.Any())
                {
                    maxVoteCount = vote.Options.Max(o => o.VoteCount);
                    
                }
                else
                {
                    
                    ModelState.AddModelError("", "Finishing the vote failed."); 
                    return StatusCode(500, ModelState);
                }


               
                var winningOption = vote.Options.Where(o => o.VoteCount == maxVoteCount);
                foreach (var user in _unitOfWork.User.GetAll().ToList())
                {
                    // Check if the user voted for any of the winning options

                    var userVotedForWinningOption = user.UserVoteAnswers != null &&
                               winningOption != null &&
                               user.UserVoteAnswers.Any(uva => winningOption.Any(vo => vo != null && vo.Option == uva.Option));
               

                    if (userVotedForWinningOption)
                    {
                        user.Wins++;
                        _unitOfWork.User.Update(user);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        _unitOfWork.User.Update(user);
                        _unitOfWork.Save();
                        user.Loses++;
                    }
                }
                
                return Ok("Successfully finished the vote!"); 
            }
            else
            {
                Console.WriteLine("here2");
                ModelState.AddModelError("", "Finishing the vote failed."); // Add a model error
                return StatusCode(500, ModelState);
            }
        }


        //TODO: finish this method, it should add the votes that the user voted for in the user's UserVoteAnswers
        [HttpPost("Vote/{id}/{voteOptionId}")]
        [Authorize]
        public async Task<IActionResult> Vote(int? id, int voteOptionId)
        {
         if (id == null || voteOptionId == null) return BadRequest();

         string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         User user = _unitOfWork.User.Get(u => u.Id == userId, "UserVoteAnswers");

            if (user == null) return BadRequest();
            
         

            
            
            var vote = _unitOfWork.Vote.Get(u => u.Id == id, "Options", true);
            if (vote == null) return NotFound();

         var voteOption = _unitOfWork.VoteOption.Get(vo => vo.Id == voteOptionId, null, true);

             if (voteOption == null)
             {
                 return BadRequest();
             }

             if (voteOption.VoteId != vote.Id)
             {

                 return BadRequest();
             }

             var hasVoted = _unitOfWork.UserVoteAnswer.Get(uva => uva.VoteId == vote.Id && uva.UserId == userId);

             if (hasVoted != null)
             {
                 return BadRequest("You have already voted for this vote.");
             }

            vote.UpdatedAt = DateTime.UtcNow.AddHours(3);
            vote.IsActive = true;
            vote.voteVotes += 1;
            voteOption.VoteCount += 1;

          

            _unitOfWork.Vote.Modify(vote);
            _unitOfWork.VoteOption.Modify(voteOption);
            _unitOfWork.Vote.Save();
            _unitOfWork.VoteOption.Save();

            var userVoteAnswer = new UserVoteAnswer
            {
                Option = voteOption.Option,
                UserId = userId,
                VoteId = vote.Id

            };
           // user.
            
            _unitOfWork.User.Save();

            _unitOfWork.UserVoteAnswer.Add(userVoteAnswer);
            _unitOfWork.UserVoteAnswer.Save();
            return Ok("Successfully voted");

        }

        

    }
}
