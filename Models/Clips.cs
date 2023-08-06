using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anime_climax_api.Models;

[Table("Clips",  Schema = "public")]
public class Clips {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; } 
    public String Caption { get; set; }
    public String Tags { get; set; }
    
    public int? Episode { get; set; }
    public long Size { get; set; }
    public float SizeMB { get; set; }
    public Animes Anime { get; set; }    
    public String Thumbnail { get; set;} 
    public String Link { get; set; }

    public DateTime DateAdded {get; set;}

}