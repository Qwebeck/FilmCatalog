using FilmApi.Utils;
using System;

namespace FilmApi.Models
{
    public class Comment: IHaveUserID
    {
        public long CommentID { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public long FilmID { get; set; }
        public string? UserID { get; set; }
        public virtual User? Author { get; set; }
        public virtual Film? CommentedFilm { get; set; }

        public Comment( string content, string userID, long filmID )
        {
            Content = content;
            UserID = userID;
            FilmID = filmID;
        }

        public Comment( CommentDTO commentDTO) 
        {
            Content = commentDTO.Content;
            PublicationDate = commentDTO.PublicationDate ?? DateTime.Now;
            FilmID = commentDTO.FilmID;
        }
    }
}
