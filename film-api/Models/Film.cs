using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FilmApi.Models
{
    public class Film
    {
        public long FilmID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string? Director { get; set; } 
        public byte[]?  Image { get; set; }
        public long UserID { get; set; }

        public virtual User? ReviewAuthor { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Mark>? Marks { get; set; }
        public Film( string title, string description, long userID, string genre)
        {
            Title = title;
            Description = description;
            UserID = userID;
            Genre = genre;
        }
        public Film () { }
    }

    public class FilmDTO 
    {
        public long FilmID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Director { get; set; }
        public byte[]? Image { get; set; }
        public string AddedBy { get; set; }
        public string Genre { get; set; }
        public float? AverageMark { get; set; }
      
        public FilmDTO( Film film)
        {
            FilmID = film.FilmID;
            Title = film.Title;
            Description = film.Description;
            Director = film.Director;
            AddedBy = film.ReviewAuthor?.Username ?? "";
            Image = film.Image;
            Genre = film.Genre;
            if (film.Marks.Count > 0)
                AverageMark = film.Marks.Average(m => m.MarkValue);
        }

    }
}
