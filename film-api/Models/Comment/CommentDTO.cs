using System;
namespace FilmApi.Models
{
    public class CommentDTO
    {
        public long CommentID { get; set; }
        public string Content { get; set; } = null!;
        public long FilmID { get; set; }
        public string? Author { get; set; }
        public DateTime? PublicationDate { get; set; }
        public CommentDTO(Comment c)
        {
            CommentID = c.CommentID;
            Author = $"{c.Author?.FirstName ?? "Unknown"} {c.Author?.LastName ?? "author"}";
            Content = c.Content;
            PublicationDate = c.PublicationDate;
            FilmID = c.FilmID;
        }
        public CommentDTO() { }
    }
}