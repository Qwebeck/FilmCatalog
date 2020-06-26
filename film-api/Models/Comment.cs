using FilmApi.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmApi.Models
{
    public class Comment: IHaveUserID
    {
        public long CommentID { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? UserID { get; set; }
        public long FilmID { get; set; }
      //  public long? PreviousCommentID { get; set; }

        public virtual User? Author { get; set; }
        public virtual Film? CommentedFilm { get; set; }
       // public virtual ICollection<Comment>? NextComments { get; set; }

        public Comment( string content, string userID, long filmID )
        {
            Content = content;
            UserID = userID;
            FilmID = filmID;
            //PreviousCommentID = previousCommentID;
        }
        public Comment () { }
    }

    public class CommentDTO 
    {
        public long CommentID { get; set; }
        public string Content { get; set; }
        public long FilmID { get; set; }
        public string? Author { get; private set; }
        public DateTime? PublicationDate { get; private set; }

        public CommentDTO(Comment c)
        {
            CommentID = c.CommentID;
            Author = $"{c.Author?.FirstName ?? ""} {c.Author?.LastName ?? ""}";
            Content = c.Content;
            PublicationDate = c.PublicationDate;
            FilmID = c.FilmID;
        }

        public CommentDTO () { }
    }
}
