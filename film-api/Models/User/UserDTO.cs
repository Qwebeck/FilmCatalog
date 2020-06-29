namespace FilmApi.Models
{

    public class UserDTO
    {
        public string? ID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Login { get; set; }
        public string? Password { get; set; }
        public UserDTO(User u)
            => (ID, FirstName, LastName, Email) = (u.UserID, u.FirstName, u.LastName, u.Email);
        public UserDTO() { }
    }
}
