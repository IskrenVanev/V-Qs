using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public VotesController(IUnitOfWork unitOfWork, IVoteRepository voteRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _voteRepository = voteRepository;
            _mapper = mapper;
         
        }
        [HttpGet]
        public IActionResult Index() //Lists all votes on the main vote page
        {
            var votes = _mapper.Map<List<VoteDTO>>(_unitOfWork.Vote.GetAll().Include(v => v.Options));
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

            if (vote == null)
                return NotFound();

            return Json(vote);
        }
        

        [HttpPost]
        public IActionResult Create([FromBody] VoteForCreateMethodDTO vote)
        //implement some kind of authentication so that when you create vote the vote's property - creator will be populated !!
        {
            if (vote == null)
                return BadRequest(ModelState);

            var newVote = _mapper.Map<Vote>(vote);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
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
        public IActionResult GetVoteResult(int? id) //this method returns the option that got most votes.
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!_voteRepository.VoteExists(id))
                return NotFound();
            
            var result =  _voteRepository.GetVoteResult(id.Value);

            if (result == null)
            {
                ModelState.AddModelError("", "Vote result not found."); 
                return StatusCode(404, ModelState); 
            }
            if (!ModelState.IsValid)
                return BadRequest();
            
            
            return Json(result); // Return a view with the vote result
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
        
        
        
        [HttpPost("Finish/{id}")]
        public  IActionResult Finish(int? id) //TODO: fix the problem with swagger
        {
            if (id == null)
            {
                return BadRequest();
            }

            
            var finished =  _voteRepository.FinishVote(id);
            long maxVoteCount = 0;
            if (finished)
            {
                var vote = _unitOfWork.Vote.Get(v => v.Id == id);
                
                if (vote.Options != null && vote.Options.Any())
                {
                    maxVoteCount = vote.Options.Max(o => o.VoteCount);
                    // Your logic when maxVoteCount is not null
                }
                else
                {
                    // Handle the case where vote.Options is null or empty
                    ModelState.AddModelError("", "Finishing the vote failed."); // Add a model error
                    return StatusCode(500, ModelState);
                }
                
                var winningOptions = vote.Options.Where(o => o.VoteCount == maxVoteCount).ToList();
                foreach (var user in _unitOfWork.User.GetAll().ToList())
                {
                    // Check if the user voted for any of the winning options
                    //TODO : Implement logic that the user should not be able to vote for more than one option
                    var userVotedForWinningOption = user.UserVoteAnswers.Any(uva => winningOptions.Any(vo => vo.Option == uva.Option));
                    var theUser =  _unitOfWork.User.Get(u => u.Id == user.Id);
                    if (userVotedForWinningOption)
                    {
                        user.Wins++;
                       
                       
                        _unitOfWork.User.Update(theUser);
                        _unitOfWork.Save();
                    }
                    else
                    {
                        _unitOfWork.User.Update(theUser);
                        _unitOfWork.Save();
                        user.Loses++;
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
        
        //TODO: Update
        
        // [HttpPost("Update/{id}{userId}" )]
        // public IActionResult Update(int? id, int? userId, VoteOption voteOption)
        // {
        //     if (id == null)
        //     {
        //         return BadRequest();
        //     }
        //     var vote = _unitOfWork.Vote.Get(u => u.Id == id);
        //     if (vote == null)
        //     {
        //         return NotFound();
        //     }
        //     var loggedInUser = _unitOfWork.User.Get(u => u.AuthId == userId.ToString());
        //
        //     if (loggedInUser == null)
        //     {
        //         return NotFound(); 
        //     }
        //     var update = _voteRepository.UpdateVote((int)id, voteOption);
        //
        //
        //     if (update)
        //     {
        //         return Ok("Successfully updated vote");
        //     }
        //     else
        //     {
        //         ModelState.AddModelError("", "Updating the vote failed."); // Add a model error
        //         return StatusCode(500, ModelState);
        //     }
        // }
        
    }
}
