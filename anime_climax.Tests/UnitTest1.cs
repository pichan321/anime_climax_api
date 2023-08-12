using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using anime_climax_api.Database;
using anime_climax_api.Controllers;

namespace anime_climax.Tests
{

    public class DataContextTests
    {
        private readonly DataContext _db = new DataContext(new DbContextOptionsBuilder<DataContext>().Options);

        [Fact]
        public void Test1()
        {
            var animeController = new AnimeController(_db);
            IActionResult a = animeController.GetAllAnimes(null);
            var response = a as OkObjectResult;
            Assert.Equal(response.Value, new List<int>());
        }
    }
}
