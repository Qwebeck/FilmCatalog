using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace FilmApi.Models
{
    public enum Roles 
    {
        Administrator,
        User
    }
    public class User
    {
        public long UserID { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public Roles Role { get; set; }
        public virtual ICollection<Mark>? Marks { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Film>? AuthoredReviews { get; set; }

        public User( string username, string email, string password, Roles role)
            => ( Username, Email, Password, Role) = ( username, email, password, role);

    }
}
