using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Dto;
using VoteAndQuizWebApi.Models;
using VoteAndQuizWebApi.Repository.IRepository;

namespace VoteAndQuizWebApi.Controllers
{
    [Route("api/Votes")]
    [ApiController]
    public class VotesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
      //  private readonly ILogger _logger;
        private readonly IVoteRepository _voteRepository;
        public VotesController(IUnitOfWork unitOfWork, IVoteRepository voteRepository)
        {
            _unitOfWork = unitOfWork;
            _voteRepository = voteRepository;
         
        }
        [HttpGet]
        public IActionResult Index()
        {
            var votesQuery = _unitOfWork.Vote.GetAll()
                .Include(v => v.Creator)
                .Include(v => v.Options);


            var votesList = votesQuery.Select(v => new VoteDTO()
            {
                Id = v.Id,
                Name = v.Name,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
                VoteEndDate = v.VoteEndDate,
                voteVotes = v.voteVotes,
                Options = v.Options,
                IsActive = v.IsActive,
                IsDeleted = v.IsDeleted,
                ShowVote = v.ShowVote,

            }).ToList();

            return Json(votesList);
        }
        [HttpGet("{id}")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            // Use the Get method from your repository to retrieve the vote by its ID
            var vote = _unitOfWork.Vote.Get(v => v.Id == id, "Options");

            if (vote == null)
            {
                return NotFound();
            }

            var votesDto = new VoteDTO
            {
                Id = vote.Id,
                Name = vote.Name,
                CreatedAt = vote.CreatedAt,
                UpdatedAt = vote.UpdatedAt,
                VoteEndDate = vote.VoteEndDate,
                voteVotes = vote.voteVotes,
                Options = vote.Options,
                IsActive = vote.IsActive,
                IsDeleted = vote.IsDeleted,
                ShowVote = vote.ShowVote,

            };

            return Json(votesDto);
        }

    }
}
