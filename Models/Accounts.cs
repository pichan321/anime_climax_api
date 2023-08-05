using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anime_climax_api.Models;

public class Accounts {

    [Key]
    public int ID { get; set; }

    [Required]
    [StringLength(200)]
    public String Email { get; set; }

    [Required]
    [StringLength(100)]
    public String Password { get; set; }


}