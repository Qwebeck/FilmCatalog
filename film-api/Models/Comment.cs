using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmApi.Models
{
    public class Comment
    {
        public long CommentID { get; set; }
        public string Content { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PublicationDate { get; set; }
        public long? UserID { get; set; }
        public long FilmID { get; set; }
        public long? PreviousCommentID { get; set; }

        private User? _author;
        public virtual User Author 
        { 
            get => _author ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Author));
            set => _author = value; 
        }
        private Film? _commentedFilm; 
        public virtual Film CommentedFilm 
        {
            get => _commentedFilm ?? throw new InvalidOperationException("Uninitialized property: " + nameof(CommentedFilm));
            set => _commentedFilm = value;
        }
        public virtual ICollection<Comment>? NextComments { get; set; }

        public Comment(long commentID, DateTime publicationDate, string content, long? userID, long filmID, long? previousCommentID = null)
        {
            CommentID = commentID;
            PublicationDate = publicationDate;
            Content = content;
            UserID = userID;
            FilmID = filmID;
            PreviousCommentID = previousCommentID;
        }            
    }
}
