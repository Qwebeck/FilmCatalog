using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FilmApi.Models
{
    public class Film
    {
        public long FilmID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string? Director { get; set; } 
        public string? ImagePath { get; set; }
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
        public long UserID {get ; set; }
        public string Image { get; set; }
        public string AddedBy { get; set; }
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
            AddedBy = film.ReviewAuthor?.Username ?? "";
            Image = UploadImage(film.ImagePath);
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

        private string UploadImage( string? imagePath ) 
        {
            if ( imagePath == "" || imagePath == null) return "";
            var sb = new StringBuilder();
            sb.Append("data:image/")
              .Append((Path.GetExtension(imagePath)).Replace(".", ""))
              .Append(";base64,")
              .Append(Convert.ToBase64String(File.ReadAllBytes(imagePath)));
            return sb.ToString();    
        }

    }
}
