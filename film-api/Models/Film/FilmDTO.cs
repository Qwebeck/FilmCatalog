using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmApi.Models
{
    public class FilmDTO
    {
        public long FilmID { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string UserID { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string[] Images { get; set; } = null!;
        public string? Director { get; set; }
        public string? AddedBy { get; set; }
        public float? AverageMark { get; set; }
        public List<CommentDTO>? Comments { get; set; }
        public FilmDTO() { }
        public FilmDTO(Film film)
        {
            UserID = film.UserID ?? "";
            FilmID = film.FilmID;
            Title = film.Title;
            Description = film.Description;
            Director = film.Director;
            AddedBy = $"{film.ReviewAuthor?.FirstName} {film.ReviewAuthor?.LastName}";
            Images = new string[] { };
            Genre = film.Genre;
            if (film.Marks.Count > 0)
                AverageMark = film.Marks.Average(m => m.MarkValue);
        }
        public FilmDTO(Film film, bool includeComments) : this(film)
        {
            if (includeComments)
                Comments = film.Comments.Select(c => new CommentDTO(c)).ToList();
        }
    }
}
