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
        
        public string Genre { get; set; }
        public string? Director { get; set; } 
        public byte[]?  Image { get; set; }
        public long UserID { get; set; }

        public virtual User? ReviewAuthor { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public Film( string title, string description, long userID, string genre)
        {
            Title = title;
            Description = description;
            UserID = userID;
            Genre = genre;
        }
    }
}
