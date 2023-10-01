using anime_climax_api.Database;
using anime_climax_api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
namespace anime_climax_api.Controllers;

[ApiController]
[Route("clip/")]
[EnableCors("AnimeClimaxOnly")]
public class ClipController: ControllerBase {
    private DataContext _db;

    public ClipController(DataContext db) {
        _db = db;
    }

    [HttpGet("view/{clip_id}")]
    public IActionResult View([FromRoute] int clip_id) {
        Clips clip = _db.Clips.Where(clip => clip_id.Equals(clip.ID)).FirstOrDefault();

        if (clip == null) {
            return BadRequest(new {Message = "Bad request", Code = 400});
        }

        clip.Views += 1;
        _db.Update(clip);
        _db.SaveChanges();
        return Ok();
    }

    [HttpGet("download/{clip_id}")]
    public IActionResult Download([FromRoute] int clip_id) {
        Clips clip = _db.Clips.Where(clip => clip_id.Equals(clip.ID)).FirstOrDefault();

        if (clip == null) {
            return BadRequest(new {Message = "Bad request", Code = 400});
        }

        clip.Downloads += 1;
        _db.Update(clip);
        _db.SaveChanges();
        return Ok();
    }
}