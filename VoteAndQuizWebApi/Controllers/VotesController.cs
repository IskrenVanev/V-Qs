using System.Text.Json;
using System.Text.Json.Serialization;
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
        private readonly IVoteOptionRepository _voteOptionRepository;
        public VotesController(IUnitOfWork unitOfWork, IVoteRepository voteRepository,IVoteOptionRepository voteOptionRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _voteRepository = voteRepository;
            _voteOptionRepository = voteOptionRepository;
            _mapper = mapper;
         
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

        //TODO: fix the problem with swagger in finish method in the VotesController
        //TODO : Implement logic that the user should not be able to vote for more than one option in finish method in votesController.

        [HttpPost("Finish/{id}")]
        public  IActionResult Finish(int? id) 
        {
            if (id == null)
            {
                return BadRequest();
            }
            var finished =  _voteRepository.FinishVote(id);
            long maxVoteCount = 0;
            
            if (finished)
            {
                var vote = _unitOfWork.Vote.Get(v => v.Id == id, "Options");
                
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
                //there may be 2 winning options, think about this situation
                var winningOption = vote.Options.Where(o => o.VoteCount == maxVoteCount);
                foreach (var user in _unitOfWork.User.GetAll().ToList())
                {
                    // Check if the user voted for any of the winning options

                    var userVotedForWinningOption = user.UserVoteAnswers != null &&
                               winningOption != null &&
                               user.UserVoteAnswers.Any(uva => winningOption.Any(vo => vo != null && vo.Option == uva.Option));
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
                Console.WriteLine("here2");
                ModelState.AddModelError("", "Finishing the vote failed."); // Add a model error
                return StatusCode(500, ModelState);
            }
        }
        


        [HttpPost("{id}")]//TODO : Fix this method because it adds new entities to the database for some reason????
        public IActionResult Vote(int? id, [FromQuery] int voteOptionId)
        {
         if (id == null || voteOptionId == null) return BadRequest();
            
         var vote = _unitOfWork.Vote.Get(u => u.Id == id, "Options");
            
         if (vote == null) return NotFound();
         var voteOption = _unitOfWork.VoteOption.Get(vo => vo.Id == voteOptionId);

         if (voteOption == null)
         {
             return BadRequest();
         }

         if (voteOption.VoteId != vote.Id)
         {
                
                return BadRequest();
         }

         var voteDTO = _mapper.Map<VoteDTO>(vote);
         var voteOptionDTO = _mapper.Map<VoteOptionDTO>(voteOption);

         voteDTO.UpdatedAt = DateTime.UtcNow.AddHours(3);
         voteDTO.IsActive = true;
         voteDTO.voteVotes += 1;
         voteOptionDTO.VoteCount += 1;

         _mapper.Map(voteDTO, vote);
        _mapper.Map(voteOptionDTO, voteOption);


         _unitOfWork.VoteOption.Update(voteOption);
         _unitOfWork.Vote.Update(vote);

         _unitOfWork.Save();

         return Ok("Successfully voted");

        }

        

        private bool Update(int? id, int? voteOptionId)
        {
            if (id == null || voteOptionId == null)
            {
                return false;
            }

            var vote = _unitOfWork.Vote.Get(u => u.Id == id, "Options");
            // var loggedInUser = _unitOfWork.User.Get(u => u.AuthId == userId);

            if (vote == null)
            {
                return false;
            }

            // Fetch the VoteOption directly from the context instead of the _unitOfWork
            var voteOption = _unitOfWork.VoteOption.Get(vo => vo.Id == voteOptionId);

            if (voteOption == null)
            {
                return false;
            }

            if (voteOption.VoteId != vote.Id)
            {
                // Ensure the voteOption belongs to the specified vote
                //  return BadRequest("Invalid vote option for the specified vote.");
                return false;
            }

            // Use AutoMapper to map your entities to DTOs
            var voteDTO = _mapper.Map<VoteDTO>(vote);
            var voteOptionDTO = _mapper.Map<VoteOptionDTO>(voteOption);

            voteDTO.UpdatedAt = DateTime.UtcNow.AddHours(3);
            voteDTO.IsActive = true;
            voteDTO.voteVotes += 1;
            voteOptionDTO.VoteCount += 1;

           // _mapper.Map(voteDTO, vote);
          //  _mapper.Map(voteOptionDTO, voteOption);


            _unitOfWork.VoteOption.Update(voteOption);
            _unitOfWork.Vote.Update(vote);

            _unitOfWork.Save();

            return true;
        }
        
    }
}
