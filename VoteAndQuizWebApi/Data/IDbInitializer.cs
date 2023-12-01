namespace VoteAndQuizWebApi.Data
{
    public interface IDbInitializer
    {
        Task SeedData();
    }
}
