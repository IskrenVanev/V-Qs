using Microsoft.AspNetCore.Identity;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
       
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<User> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public void SeedData()
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == "admin@iskren.com");

            if (user == null)
            {
                var newUser = new User
                {
                    UserName = "admin@iskren.com",
                    Email = "admin@iskren.com",
                    PhoneNumber = "123123123",
                };

                var result = _userManager.CreateAsync(newUser, "Qqq123*").GetAwaiter().GetResult();

                if (result.Succeeded)
                {
                    user = newUser; // Use the same instance created by UserManager
                }
                else
                {
                    throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                if (!_db.Quizzes.Any())
                {
                    // Create a list of quizzes to seed the database
                    var quizToSeed = new List<Quiz>
                    {
                         new Quiz
                         {
                            Name = "Quiz 1",
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
                            CorrectOption = new WinnerQuizOption { Answer = "Answer 2"  }
                                              // Add more quizzes as needed
                         }
                    };
                    // Add quizzes to the database
                    _db.Quizzes.AddRange(quizToSeed);
                    _db.SaveChanges();
                }

                if (!_db.Votes.Any())
                {
                    var votesToSeed = new List<Vote>
                    {
                        new Vote
                        {
                            Name = "Vote 1",
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
        }
    }
}
