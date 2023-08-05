using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anime_climax_api.Models;

[Table("Animes", Schema = "public")]
public class Animes {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set;}

    [Required]
    public String Name { get; set;}
    public String Icon { get; set;}
    public String Background { get; set;}
    
}

