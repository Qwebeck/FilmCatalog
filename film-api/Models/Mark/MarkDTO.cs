using System;
namespace FilmApi.Models
{
    public class MarkDTO
    {
        public float Mark { get; set; }
        public long FilmID { get; set; }
        public string? AuthoredBy { get; set; }
        public string? FilmTitle { get; set; }
        public MarkDTO(Mark mark)
        {
            Mark = mark.MarkValue;
            try
            {
                FilmTitle = mark.MarkedFilm.Title;
            }
            catch ( InvalidOperationException )
            {
                FilmTitle = "";
            }
            AuthoredBy = $"{mark.Author?.FirstName ?? "Unknown"} {mark.Author?.LastName ?? "user"}";
            FilmID = mark.FilmID;
        }

        public MarkDTO() { }
    }
}
