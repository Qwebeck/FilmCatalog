﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmApi.Models
{
    public class Comment
    {
        public long CommentID { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public long? UserID { get; set; }
        public long FilmID { get; set; }
        public long? PreviousCommentID { get; set; }

        public virtual User Author { get; set; }
        public virtual Film CommentedFilm { get; set; }
        public virtual ICollection<Comment>? NextComments { get; set; }

        public Comment( string content, long? userID, long filmID, long? previousCommentID = null)
        {
            Content = content;
            UserID = userID;
            FilmID = filmID;
            PreviousCommentID = previousCommentID;
        }            
    }
}
