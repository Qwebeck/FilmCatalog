using System;
using System.ComponentModel.DataAnnotations;
namespace FilmApi.Models
{
    public class Mark
    {
        public long MarkID { get; set; }
        [Range(10, 1000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public float MarkValue { get; set; }
        public long? UserID { get; set; }
        public long FilmID { get; set; }
        private Film? _markedFilm;
        public virtual Film MarkedFilm 
        { 
            get => _markedFilm ?? throw new InvalidOperationException("Uninitialized property: " + nameof(MarkedFilm));
            set => _markedFilm = value;
        }
        private User? _author;
        public virtual User Author 
        { 
            get => _author ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Author));
            set => _author = Author;
        }
    }
}
