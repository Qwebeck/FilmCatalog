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
        private Film? _markedFilm;
        public virtual Film MarkedFilm 
        { 
            get => _markedFilm ?? throw new InvalidOperationException("Unitialized value of property MarkedFilm"); 
            set => _markedFilm = value; 
        }
        public virtual User? Author { get; set; }
    }
}
