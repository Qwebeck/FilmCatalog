using FilmApi.Utils;
using System.Collections.Generic;
using System.Linq;

namespace FilmApi.Models
{
    public class Film: IHaveUserID
    {
        public long FilmID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string? Director { get; set; } 
        public string UserID { get; set; }
        public virtual User? ReviewAuthor { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
        public Film( string title, string description, string userID, string genre)
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
        public string? UserID {get ; set; }
        public string[] Images { get; set; }
        public string? AddedBy { get; set; }
        public string Genre { get; set; }
        public float? AverageMark { get; set; }
        public List<CommentDTO>? Comments {get; set;}
      
        public FilmDTO( Film film )
        {
            UserID = film.UserID;
            FilmID = film.FilmID;
            Title = film.Title;
            Description = film.Description;
            Director = film.Director;
            AddedBy = $"{film.ReviewAuthor.FirstName} {film.ReviewAuthor.LastName}" ;
            Genre = film.Genre;
            if ( film.Marks.Count > 0 )
                AverageMark = film.Marks.Average(m => m.MarkValue);
        }

        public FilmDTO( Film film, bool includeComments ): this(film)
        {
            if ( includeComments )
                Comments = film.Comments.Select(c => new CommentDTO(c)).ToList();
        }

        public FilmDTO () { }
    }
}
