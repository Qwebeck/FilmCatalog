using FilmApi.Utils;
using System;
using System.ComponentModel.DataAnnotations;
namespace FilmApi.Models
{
    public class Mark: IHaveUserID
    {
        public long MarkID { get; set; }
        [Range(0, 5,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public float MarkValue { get; set; }
        public string? UserID { get; set; }
        public long FilmID { get; set; }
        public virtual Film MarkedFilm { get; set; }
        public virtual User? Author { get; set; }
    }

    public class MarkDTO 
    {
        public float Mark { get; set; }
        public long FilmID { get; set; }
        public string? AuthoredBy { get; set; }
        public string? FilmTitle { get; set; }
        public MarkDTO (Mark mark) 
        {
            Mark = mark.MarkValue;
            FilmTitle = mark.MarkedFilm.Title;
            AuthoredBy = $"{mark.Author?.FirstName ?? "Unknown"} {mark.Author?.LastName ?? "user"}";
            FilmID = mark.FilmID;
        }

        public MarkDTO() { }
    }
}
