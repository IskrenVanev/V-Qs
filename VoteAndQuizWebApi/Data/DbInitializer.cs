using Microsoft.AspNetCore.Identity;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
       
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<User> userManager,
            
            ApplicationDbContext db)
        {
            
            _userManager = userManager;
            _db = db;

        }

        public async Task SeedData()
        {
            if (await _userManager.FindByEmailAsync("admin@iskren.com") == null)
            {

                var FirstUser = _userManager.CreateAsync(new User
                {
                    UserName = "admin@iskren.com",
                    Email = "admin@iskren.com",
                    
                    PhoneNumber = "123123123",
                    


                }, "Qqq123*").GetAwaiter().GetResult();

                User user = _db.Users.FirstOrDefault(u => u.Email == "admin@iskren.com");
              
               
                if (!_db.Quizzes.Any())
                {
                    // Create a list of quizzes to seed the database
                    var quizzesToSeed = new List<Quiz>
                    {
                        new Quiz
                        {
                            Name = "Quiz 1",
                            //CreatorId =newUser.CustomUserId,
                            Creator = user,
                            CreatedAt = DateTime.Now,
                            QuizEndDate = DateTime.Now.AddDays(7),
                            quizVotes = 0,
                            IsActive = true,
                            IsDeleted = false,
                            ShowQuiz = true,
                            // Add options and correct option as needed
                            Options = new List<UserQuizAnswer>
                            {
                                new UserQuizAnswer { UserAnswer = "Answer 1" },
                                new UserQuizAnswer { UserAnswer = "Answer 2" },
                                new UserQuizAnswer { UserAnswer = "Answer 3" }

                            },
                            CorrectOption = new QuizOption { Answer = "Answer 2"  }
                        },
                        // Add more quizzes as needed
                    };

                    // Add quizzes to the database
                    _db.Quizzes.AddRange(quizzesToSeed);
                    _db.SaveChanges();
                }

                if (!_db.Votes.Any())
                {
                    var votesToSeed = new List<Vote>
                    {
                        new Vote
                        {
                            Name = "Vote 1",
                            //CreatorId = newUser.CustomUserId,
                            Creator = user,
                            CreatedAt = DateTime.Now,
                            VoteEndDate = DateTime.Now.AddDays(7),
                            voteVotes = 0,
                            IsActive = true,
                            IsDeleted = false,
                            ShowVote = true,
                            Options = new List<VoteOption>
                            {
                                new VoteOption {Option = "Option 1"},
                                new VoteOption {Option = "Option 2"},
                                new VoteOption {Option = "Option 3"}
                            }

                        }
                    };
                    _db.Votes.AddRange(votesToSeed);
                    _db.SaveChanges();
                }
            }
            // var newUser =  _userManager.CreateAsync(new User
            //   {
            //       UserName = "admin@iskren.com",
            //       Email = "admin@iskren.com",
            //       Wins = 0, // Set initial wins count
            //       Loses = 0, // Set initial loses count
            //
            //
            //   }, "Qqq123*").GetAwaiter().GetResult();



        }
    }
}
