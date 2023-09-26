using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<QuizOption> QuizOptions { get; set; }
        public DbSet<VoteOption> VoteOptions { get; set; }
        public DbSet<UserQuizAnswer> UserQuizAnswers { get; set; }
        public DbSet<UserVoteAnswer> UserVoteAnswers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)//TODO:Fix entity relations
        {


            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasConversion(
                    v => v.ToByteArray(),
                    v => new Guid(v)
                );



            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Options)
                .WithOne(qa => qa.Quiz)
                .HasForeignKey(qa => qa.QuizId);


            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.CorrectOption)
                .WithOne(qo => qo.Quiz)
                .HasForeignKey<QuizOption>(qa => qa.QuizId);



            base.OnModelCreating(modelBuilder);
        }

        public void SeedData()
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(), // Generate a new unique ID for the user
                UserName = "Username", // Set the username
                AuthId = "AuthId", // Set the AuthId
                Wins = 0, // Set initial wins count
                Loses = 0, // Set initial loses count
            };

            if (!Quizzes.Any())
            {
                // Create a list of quizzes to seed the database
                var quizzesToSeed = new List<Quiz>
                {
                    new Quiz
                    {
                        Name = "Quiz 1",
                        CreatorId =newUser.Id,
                        Creator = newUser,
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
                Quizzes.AddRange(quizzesToSeed);
                SaveChanges();
            }

            if (!Votes.Any())
            {
                var votesToSeed = new List<Vote>
                {
                    new Vote
                    {
                        Name = "Vote 1",
                        CreatorId = newUser.Id,
                        Creator = newUser,
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
                Votes.AddRange(votesToSeed);
                SaveChanges();
            }

        }
    }
    
}
