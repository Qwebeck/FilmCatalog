﻿using System.Collections.Generic;
using FilmApi.Utils;
using FilmApi.Utils.Attributes;
namespace FilmApi.Models
{
    public class User: IHaveUserID
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set;  }
        
        public virtual ICollection<Mark>? Marks { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Film>? AuthoredReviews { get; set; }
        public User(string userID, string firstName, string lastName, string email)
            => (UserID, FirstName, LastName, Email) = (userID, firstName, lastName, email);        
        public User() { }
    }


    public class UserDTO
    {
        public string? ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Login { get; set; }
        [Credentials]
        public string? Password { get; set; }
        public UserDTO(User u)
            => (ID, FirstName, LastName, Email ) = ( u.UserID, u.FirstName, u.LastName, u.Email );
        public UserDTO () { }
    }
}
