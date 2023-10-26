using Microsoft.EntityFrameworkCore;
using VoteAndQuizWebApi.Data;
using VoteAndQuizWebApi.Repository.IRepository;
using VoteAndQuizWebApi.Repository;
using Microsoft.AspNetCore.Identity;

using VoteAndQuizWebApi.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddAuthorization();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<IVoteOptionRepository, VoteOptionRepository>();
//builder.Services.AddScoped<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<ApplicationDbContext>();
dbContext.SeedData(); // Call the SeedData method here

app.MapControllers();

app.Run();



//TODO: fix the problem with swagger in finish method in the VotesController. It does not add the wins/loses to the user that has logged in
//TODO: Implement authentication for creating a vote and assign Creator property to be the logged in user, then you can test the finish method.





//TODO:Finally when you are ready with everything, learn how to use gRPC and connect your part of the project to Jam's project!!!








//TODO:Implement logic for the date in other countries too in the controllers.