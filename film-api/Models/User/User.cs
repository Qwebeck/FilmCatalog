using System.Collections.Generic;
using FilmApi.Utils;
namespace FilmApi.Models
{
    public class User: IHaveUserID
    {
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public string UserID { get; set; }
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set;  }
        
        public virtual ICollection<Mark>? Marks { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Film>? AuthoredReviews { get; set; }
        public User(string userID, string firstName, string lastName, string email)
            => (UserID, FirstName, LastName, Email) = (userID, firstName, lastName, email);
    }
}
