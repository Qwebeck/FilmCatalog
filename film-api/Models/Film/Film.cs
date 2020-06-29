using FilmApi.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public string? UserID { get; set; }
        public virtual User? ReviewAuthor { get; set; }
        private ICollection<Comment>? _comments;
        public virtual ICollection<Comment> Comments {
            set => _comments = value;
            get => _comments ?? new List<Comment>();
        }
        private ICollection<Mark>? _marks;
        public virtual ICollection<Mark> Marks 
        {
            set => _marks = value;
            get => _marks ?? new List<Mark>();
        }
        public Film( string title, string description, string userID, string genre)
        {
            Title = title;
            Description = description;
            UserID = userID;
            Genre = genre;
        }
        public Film (FilmDTO filmDTO) 
        {
            Title = filmDTO.Title;
            Description = filmDTO.Description;
            UserID = filmDTO.UserID;
            Genre = filmDTO.Genre;
            Director = filmDTO.Director;
        } 
    }
}
