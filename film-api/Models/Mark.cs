using System;
using System.ComponentModel.DataAnnotations;
namespace FilmApi.Models
{
    public class Mark
    {
        public long MarkID { get; set; }
        [Range(0, 5,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public float MarkValue { get; set; }
        public long? UserID { get; set; }
        public long FilmID { get; set; }
        public virtual Film? MarkedFilm { get; set; }
        public virtual User? Author { get; set; }
    }

    public class MarkDTO 
    {
        public float MarkValue { get; private set; }
        public string FilmTitle { get; private set; }
        public string AuthoredBy { get; private set; }

        public MarkDTO (Mark mark) 
        {
            MarkValue = mark.MarkValue;
            FilmTitle = mark.MarkedFilm.Title;
            AuthoredBy = mark.Author.Username;
        }
    }
}
