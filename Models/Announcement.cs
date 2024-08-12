namespace Company_X.Models
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public string? Title { get; set; }
        public string? Content {  get; set; }
        public DateTime DateUpload { get; set; }
        public int Copyright { get; set; }
    }
}
