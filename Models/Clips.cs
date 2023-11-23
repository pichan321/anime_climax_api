using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

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
    public Animes? Anime { get; set; }    
    public String Thumbnail { get; set;} 
    public String Link { get; set; }

    public DateTime DateAdded {get; set;}

    // [HotChocolate.Types.DefaultValue("")]
    // [DefaultValue("")]
    public String Timestamps {get; set;}

    // [HotChocolate.Types.DefaultValue(0)]
    // [DefaultValue(0)]
    public int Views {get; set;}

    // [HotChocolate.Types.DefaultValue(0)]
    // [DefaultValue(0)]
    public int Downloads {get; set;}
}