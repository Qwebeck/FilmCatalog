using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilmApi.Models
{
    public class Film
    {
        public long FilmID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Director { get; set; } 
        public byte[]?  Image { get; set; }
        public long UserID { get; set; }
        private User? _reviewAuthor;
        public virtual User ReviewAuthor 
        { 
            get => _reviewAuthor ?? throw new InvalidOperationException("Uninitialized property: " + nameof(ReviewAuthor));
            set => _reviewAuthor = value;
        }
        public virtual ICollection<Comment>? Comments { get; set; }
        public Film(long filmID, string title, string description, long userID)
        {
            FilmID = filmID;
            Title = title;
            Description = description;
            UserID = userID;
        }
    }
}
