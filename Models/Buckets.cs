using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anime_climax_api.Models;

public class Buckets {
    const float MB = 1024 * 1024;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    public String BucketName { get; set; } = "";

    [Required]
    public String Token { get; set; } = "";

    public String? ShareLink { get; set; }

    public float Usage { get; set; } = 0.0f;
    
    public float Capacity { get; set; } = 25000 * MB;

    public required Accounts Account { get; set; }
}