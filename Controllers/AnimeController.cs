using Microsoft.AspNetCore.Mvc;
using anime_climax_api.Database;
using anime_climax_api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using anime_climax_api.Utilities;
using anime_climax_api.Binding;
using uplink.NET.Services;
using uplink.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace anime_climax_api.Controllers;

[ApiController]
[Route("anime")]
public class AnimeController : ControllerBase {
    private const String SECRET = "pichan327";
    private readonly int RESULT_PER_PAGE = 15;
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
        List<Clips> clips =_db.Clips.Include(clip => clip.Anime).Where(c => c.Anime.ID == id).ToList();

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


    [HttpPost("/anime/clip/add-clip")]
      public async Task<IActionResult> AddNewClip(int id, [FromForm] NewClip clip)
        {
            try
            {
                if (clip.File.Length == 0){return BadRequest(); }

                byte[] buffer = new byte[10 * 1024 * 1024];
             
                Uploader uploader = new Uploader(_db);
                float fileSizeMB = clip.File.Length / (1024 * 1024);
                Buckets bucket = uploader.PickBucket(fileSizeMB).Result;
                if (bucket == null) {
                    return StatusCode(StatusCodes.Status500InternalServerError, new {Message = "Internal server error"});
                }
                
                
                var uploadService = uploader.Upload(bucket, clip).Result;
                
                const int bufferSize = 10 * 1024 * 1024;
                byte[] bufferArray = new byte[bufferSize];
                uint part = 1;
                Stream stream = clip.File.OpenReadStream();
                int n;
                while ((n = stream.Read(bufferArray, 0, bufferSize)) > 0) {
                        await uploadService.Item3.UploadPartAsync(bucket.BucketName, uploadService.Item1, uploadService.Item2.UploadId, part, bufferArray[0..n]);
                        part++;
                        Console.WriteLine(n);
                }
            
                await uploadService.Item3.CommitUploadAsync(bucket.BucketName, uploadService.Item1, uploadService.Item2.UploadId, new CommitUploadOptions());
                
                bucket.Usage += fileSizeMB;
                Animes anime = _db.Animes.Where(anime => anime.ID == clip.AnimeID).FirstOrDefault();
                Clips clipToAdd = new Clips{
                    Caption = clip.Caption,
                    Tags = clip.Tags,
                    Anime = anime,
                    Thumbnail = clip.Thumbnail,
                    Size = clip.File.Length,
                    SizeMB = fileSizeMB,
                    Link = bucket.ShareLink + uploadService.Item1 + "?wrap=0",
                    DateAdded = DateTimeOffset.UtcNow.UtcDateTime,
                };
                _db.Buckets.Add(bucket);
                _db.Clips.Add(clipToAdd);
                await _db.SaveChangesAsync();
                return Ok(clipToAdd);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }
    }

    [HttpDelete("/anime/{id}/clip/delete")]
    public IActionResult DeleteClip() {

        return Ok();
    }



    [HttpGet("/test")]
    public IActionResult Test() {
        Uploader uploader = new Uploader(_db);
        Access access = new Access("15M6fjomdWMwh4cdbZx5YmDQpQsc8EN73sYKcfLodh6yz6PXEbNJe1WKFvKrwMotebVhRWPiihQoPEuKkaEt1reW5WhPwipmRZnqcfnA6ATKsf6ApbkjvjtJaG1YRZMshrYoTi1Xp5CpFHMLHBaHHBaJNTZjXKfwLDaNFFyE9cGjWZpSYkFzu2Xhc3joQ3QyyakkjETrDR2fWkuxjAJNGGaBpRTMdXwJp3awCMV5jgMsvqA47qC3ocrSBM3vn7bBjt22BResHwrEvJCMC3bqt8oR1NkZc9ip5");
        // uploader.Upload();
        return Ok();
    }

}

