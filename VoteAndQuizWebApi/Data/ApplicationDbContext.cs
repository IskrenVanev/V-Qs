using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Models;

namespace VoteAndQuizWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options) : base(options)
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

           // modelBuilder.Entity<User>().HasKey(u => u.CustomUserId);
            // modelBuilder.Entity<User>()
            //     .Property(u => u.Id)
            //     .HasConversion(
            //         v => v.ToByteArray(),
            //         v => new Guid(v)
            //     );

            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Creator)
                .WithMany()
                .HasForeignKey(q => q.CreatorId);

            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Options)
                .WithOne(qa => qa.Quiz)
                .HasForeignKey(qa => qa.QuizId);


            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.CorrectOption)
                .WithOne(qo => qo.Quiz)
                .HasForeignKey<QuizOption>(qa => qa.QuizId);

            modelBuilder.Entity<Vote>()
                .HasMany(v => v.Options)
                .WithOne(vo => vo.Vote)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserVoteAnswer>()
                .HasOne(uva => uva.Vote)
                .WithMany(v => v.UserVoteAnswers)
                .HasForeignKey(uva => uva.VoteId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserQuizAnswer>()
            //    .HasOne(uqa => uqa.Quiz)
            //    .WithMany(q => q.Options)
            //    .HasForeignKey(uqa => uqa.QuizId)
            //    .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

        //public async Task SeedData()
        //{
        //    if (await _userManager.FindByEmailAsync("admin@iskren.com") == null)
        //    {
        //        var newUser = new User
        //        {
        //            UserName = "admin@iskren.com",
        //            Email = "admin@iskren.com",
        //            Wins = 0, // Set initial wins count
        //            Loses = 0, // Set initial loses count
        //        };

        //        // Create the user
        //        var result = await _userManager.CreateAsync(newUser);

        //        // Check if the user creation was successful
        //        if (result.Succeeded)
        //        {
        //            // Set the password for the created user
        //            var setPasswordResult = await _userManager.AddPasswordAsync(newUser, "YourDesiredPassword");
        //            if (setPasswordResult.Succeeded)
        //            {
        //                // Password set successfully
        //            }
        //            else
        //            {
        //                throw new InvalidOperationException();
        //            }
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException();
        //        }
        //        if (!Quizzes.Any())
        //        {
        //            // Create a list of quizzes to seed the database
        //            var quizzesToSeed = new List<Quiz>
        //            {
        //                new Quiz
        //                {
        //                    Name = "Quiz 1",
        //                    //CreatorId =newUser.CustomUserId,
        //                    Creator = newUser,
        //                    CreatedAt = DateTime.Now,
        //                    QuizEndDate = DateTime.Now.AddDays(7),
        //                    quizVotes = 0,
        //                    IsActive = true,
        //                    IsDeleted = false,
        //                    ShowQuiz = true,
        //                    // Add options and correct option as needed
        //                    Options = new List<UserQuizAnswer>
        //                    {
        //                        new UserQuizAnswer { UserAnswer = "Answer 1" },
        //                        new UserQuizAnswer { UserAnswer = "Answer 2" },
        //                        new UserQuizAnswer { UserAnswer = "Answer 3" }

        //                    },
        //                    CorrectOption = new QuizOption { Answer = "Answer 2"  }
        //                },
        //                // Add more quizzes as needed
        //            };

        //            // Add quizzes to the database
        //            Quizzes.AddRange(quizzesToSeed);
        //            SaveChanges();
        //        }

        //        if (!Votes.Any())
        //        {
        //            var votesToSeed = new List<Vote>
        //            {
        //                new Vote
        //                {
        //                    Name = "Vote 1",
        //                    //CreatorId = newUser.CustomUserId,
        //                    Creator = newUser,
        //                    CreatedAt = DateTime.Now,
        //                    VoteEndDate = DateTime.Now.AddDays(7),
        //                    voteVotes = 0,
        //                    IsActive = true,
        //                    IsDeleted = false,
        //                    ShowVote = true,
        //                    Options = new List<VoteOption>
        //                    {
        //                        new VoteOption {Option = "Option 1"},
        //                        new VoteOption {Option = "Option 2"},
        //                        new VoteOption {Option = "Option 3"}
        //                    }

        //                }
        //            };
        //            Votes.AddRange(votesToSeed);
        //            SaveChanges();
        //        }
        //    }
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
    

