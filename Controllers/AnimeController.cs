using Microsoft.AspNetCore.Mvc;
using anime_climax_api.Database;
using anime_climax_api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using anime_climax_api.Utilities;
using uplink.NET.Services;
using uplink.NET.Models;
namespace anime_climax_api.Controllers;

[ApiController]
[Route("anime")]
public class AnimeController : ControllerBase {
    private const String SECRET = "pichan327";
    private readonly DataContext _db;

    public AnimeController(DataContext db)
    {
        _db = db;
    }

    [HttpGet("/anime/")]
    public String Hello() {
        return "Hello there";
    }

    [HttpGet("/anime/{id}")]
    public IActionResult GetAnime(int id) {
        Animes anime = _db.Animes.Find(id);
        if (anime == null) {
            return NotFound();
        }
        return Ok(anime);
    }

    [HttpGet("/anime/{id}/clips")]
    public IActionResult GetClips(int id) {
        List<Clips> clips = _db.Clips.Where(c => c.Anime.ID == id).ToList();

        return Ok(clips);
    }

    [HttpGet("/anime/animes")]
    public IActionResult GetAllAnimes()
    {
        List<Animes> allAnimes = _db.Animes.ToList();
        return Ok(allAnimes);
    }

    [HttpPost("/anime/add")]
    public IActionResult AddNewAnime( [FromBody] Animes anime) {
        try {
            _db.Animes.Add(new Animes{
            Name = anime.Name,
            Icon = anime.Icon,
            Background = anime.Background
            });
            _db.SaveChanges();
            return Ok(new {Message = "Added", Code = 200});
        } catch (Exception e) {
            return new ObjectResult(new {Message = "Unable to add new anime"}){StatusCode = StatusCodes.Status500InternalServerError};
        }
    }

[HttpDelete("/anime/delete/{id}")]
public IActionResult DeleteAnime(int id)
{
    try
    {
        Animes anime = _db.Animes.FirstOrDefault(anime => anime.ID == id);

        if (anime != null)
        {
            _db.Animes.Remove(anime);
            _db.SaveChanges();

            return Ok(new { Message = "Anime deleted successfully." });
        }
        else
        {
            return NotFound(new { Message = "Anime not found." });
        }
    }
    catch (Exception e)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = e.Message });
    }
}

    [HttpPost("/anime/{id}/clip/add-clip")]
    public IActionResult AddNewClip() {
        return Ok();
    }

    [HttpDelete("/anime/{id}/clip/delete")]
    public IActionResult DeleteClip() {

        return Ok();
    }



    [HttpGet("/test")]
    public IActionResult Test() {
        Uploader uploader = new Uploader();
        Access access = new Access("15M6fjomdWMwh4cdbZx5YmDQpQsc8EN73sYKcfLodh6yz6PXEbNJe1WKFvKrwMotebVhRWPiihQoPEuKkaEt1reW5WhPwipmRZnqcfnA6ATKsf6ApbkjvjtJaG1YRZMshrYoTi1Xp5CpFHMLHBaHHBaJNTZjXKfwLDaNFFyE9cGjWZpSYkFzu2Xhc3joQ3QyyakkjETrDR2fWkuxjAJNGGaBpRTMdXwJp3awCMV5jgMsvqA47qC3ocrSBM3vn7bBjt22BResHwrEvJCMC3bqt8oR1NkZc9ip5");
        UploadInfo bucketList = uploader.Upload();
        return Ok(bucketList);
    }

}

