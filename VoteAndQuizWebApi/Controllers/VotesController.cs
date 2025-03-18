using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
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
    [Authorize]
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
            var votes = _mapper.Map<List<VoteDTO>>(_unitOfWork.Vote.GetAll(v => v.IsActive).Include(v => v.Options));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Json(votes);
        }

        [HttpGet("Details/{id}")]
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

        [HttpPost("Create")]
        [Authorize]
        public IActionResult Create([FromBody] VoteForCreateMethodDTO vote)
        //implement some kind of authentication so that when you create vote the vote's property - creator will be populated !!
         {

            if (vote == null || vote.Options == null || vote.Options.Count < 2)
            {
                ModelState.AddModelError("", "Vote must have at least 2 options.");
                return BadRequest(ModelState);
            }

            if (vote.VoteEndDate < DateTime.UtcNow.AddDays(1))
            {
                ModelState.AddModelError("", "Vote end date must be at least one day from today.");
                return BadRequest(ModelState);
            }

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
                UserVoteAnswers = new List<UserVoteAnswer>(),//Here, I should have a winner option (or tie)
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

            return Ok("Successfully created");
        }

        [HttpGet("Result/{id}")]
        public IActionResult GetVoteResult(int? id)
        {
            if (id == null)
            {
                return BadRequest("Vote ID cannot be null.");
            }

            if (!_voteRepository.VoteExists(id.Value))
            {
                return NotFound("Vote not found.");
            }

            var result = _voteRepository.GetVoteResult(id.Value);

            if (result == null || !result.Any())
            {
                ModelState.AddModelError("", "No results found for this vote.");
                return StatusCode(404, ModelState);
            }

            var vote = _unitOfWork.Vote.Get(v => v.Id == id);
            if (vote.IsDeleted)
            {
                return BadRequest("This vote has been deleted.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the list of VoteOption to a list of VoteOptionDTO
            var voteOptionDtos = result.Select(option => new VoteOptionDTO
            {
                voteCount = option.VoteCount,
                Option = option.Option,
            }).ToList();

            return Ok(voteOptionDtos);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (!_voteRepository.VoteExists(id))
                return NotFound();

            if (id == null || id == 0)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Id == userId);
            if (user == null) return Unauthorized("Log in to delete a vote");

            Vote? voteFromDb = _unitOfWork.Vote.Get(u => u.Id == id);

            if (voteFromDb == null)
            {
                return NotFound();
            }

            if (voteFromDb.IsDeleted)
                return BadRequest("This vote is already deleted!");

            if (voteFromDb.CreatorId != userId)
            {
                return Unauthorized("You are not authorized to delete this vote.");
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

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Id == userId, "UserVoteAnswers");
            if (user == null) return Unauthorized("Log in to finish a vote");

            var vote = _unitOfWork.Vote.Get(v => v.Id == id, "Options");

            if (userId != vote.CreatorId)
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

                var winningOptions = vote.Options.Where(o => o.VoteCount == maxVoteCount);
                foreach (var u in _unitOfWork.User.GetAll(null, "UserVoteAnswers").ToList())
                {
                    // Check if the user voted for any of the winning options
                    var userVotedForWinningOption = u.UserVoteAnswers != null &&
                        winningOptions != null &&
                        u.UserVoteAnswers.Any(uva =>
                            winningOptions.Any(wo =>
                                wo.VoteId == uva.VoteId && 
                                wo.Option == uva.Option 
                            )
                        );

                    if (userVotedForWinningOption)
                    {
                        u.Wins++;
                        _unitOfWork.User.Update(u);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        u.Loses++;
                        _unitOfWork.User.Update(u);
                        _unitOfWork.Save();
                    }
                }
                
                return Ok("Successfully finished the vote!"); 
            }
            else
            {
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

             _unitOfWork.User.Save();
            
             _unitOfWork.UserVoteAnswer.Add(userVoteAnswer);
             _unitOfWork.UserVoteAnswer.Save();

             return Ok("Successfully voted");
        }
    }
}
