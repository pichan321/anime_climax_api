namespace anime_climax_api.Binding;

public class NewClip {
        public IFormFile File  { get; set; }
        public int AnimeID { get; set; }
        public String Caption { get; set; }
        public String? Tags { get; set; } = "";
        public String? Thumbnail { get; set;} = "";
}