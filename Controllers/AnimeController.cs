using Microsoft.AspNetCore.Mvc;
using anime_climax_api.Database;
using anime_climax_api.Models;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using anime_climax_api.Utilities;
using anime_climax_api.Binding;
using uplink.NET.Services;
using uplink.NET.Models;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
namespace anime_climax_api.Controllers;

[ApiController]
[Route("anime")]
public class AnimeController : ControllerBase {
    private const String SECRET = "pichan327";
    private readonly int RESULT_PER_PAGE = 16;
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
        int clipCounts = _db.Clips.Count(clip => clip.Anime.ID == anime.ID);
        dynamic response = new {
            anime,
            clipCounts = clipCounts
        };
        return Ok(response);
    }

    [HttpGet("/anime/{id}/clips")]
    public IActionResult GetClips(int id, [FromQuery] int page = 1) {
        if (page <= 0) {
            return Ok(new List<Animes>());
        }
        int skip = page == 1 ? 0 : RESULT_PER_PAGE * (page - 1);
        List<Clips> clips =_db.Clips.Include(clip => clip.Anime).Where(c => c.Anime.ID == id).Skip(skip).Take(RESULT_PER_PAGE).ToList();

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
    [RequestSizeLimit(100_000_000)]
      public async Task<IActionResult> AddNewClip(int id, [FromForm] NewClip clip)
        {
            try
            {
                if (clip.File.Length == 0){return BadRequest();}

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
                
                Console.WriteLine("Episode: " + clip.Episode);
                bucket.Usage += fileSizeMB;
                Animes anime = _db.Animes.Where(anime => anime.ID == clip.AnimeID).FirstOrDefault();
                Clips clipToAdd = new Clips{
                    Caption = clip.Caption,
                    Tags = clip.Tags != null ? clip.Tags : "",
                    Anime = anime,
                    Thumbnail = clip.Thumbnail,
                    Episode = clip.Episode > 0 ? clip.Episode : 0,
                    Size = clip.File.Length,
                    SizeMB = fileSizeMB,
                    Link = bucket.ShareLink + uploadService.Item1 + "?wrap=0",
                    DateAdded = DateTimeOffset.UtcNow.UtcDateTime,
                };
                _db.Buckets.Update(bucket);
                _db.Clips.Add(clipToAdd);
                _db.SaveChanges();
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

[HttpPost("/anime/image")]
    public async Task<IActionResult> ProcessImage(IFormFile file)
    {
        // Ensure the file is not null and is a valid image
        if (file == null || file.Length == 0 )
        {
            return BadRequest("Invalid image file");
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            using (Image<Rgba32> image = Image.Load<Rgba32>(memoryStream))
            // using (MemoryStream outputMemoryStream = new MemoryStream())
            // {
            //     // Process the image (resize in this example)
            //     image.Mutate(ctx => ctx.Resize(new ResizeOptions { Size = new Size(800, 600), Mode = ResizeMode.Max }));

            //     // Save the modified image to the output memory stream
            //     image.Save(outputMemoryStream, new PngEncoder());

            //     // Create an HTTP response with the modified image data
            //     byte[] modifiedImageBytes = outputMemoryStream.ToArray();
            return Ok(image);
                // return File(modifiedImageBytes, "image/png"); // Set the appropriate content type
            }
        }
    


    [HttpGet("/test")]
    public IActionResult Test() {
    
      
        Access access = new Access("15M6fjomdWMwh4cdbZx5YmDQpQsc8EN73sYKcfLodh6yz6PXEbNJe1WKFvKrwMotebVhRWPiihQoPEuKkaEt1reW5WhPwipmRZnqcfnA73kUviGwRcB4onirZgySzZJ881rUVWrwcmg4LfpwEXubRBvtavDDfGH6q5hdjxq2K2WcXGWYoaVMViRjijtD8qZHvmq39Jo6mqo5KqV2URGkN2vJFNpjnmSPaa16Hu3wyWJoTGgEHW7irkmPcC5QhshVk9SQvyMzyJFrQevinDigriF2uN6XGYXwt");
        BucketService bucketService = new BucketService(access);
        return Ok();
    }

}

