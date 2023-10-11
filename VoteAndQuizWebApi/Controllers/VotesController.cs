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
        //TODO: Create, GetVoteResult, Finish, Delete, Update

        [HttpPost]
        public IActionResult Create([FromBody] VoteForCreateMethodDTO vote)//use another DTO for creation!!
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



    }
}
