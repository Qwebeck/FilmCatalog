
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.DAL;
using FilmApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using FilmApi.Utils.JSONConverters;
using FilmApi.AuthorityProviders;
using System.Security.Claims;

namespace FilmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;
        private readonly OktaMiddleware oktaMiddleware = new OktaMiddleware();
       
        private JsonSerializerOptions converterOptions = new JsonSerializerOptions();

        public UsersController(Context context)
        {
            _context = context;
            converterOptions.Converters.Add(new UserDTOConverter());
        }
        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTO user)
        {
            if ( UserExists(user)) 
            {
                return BadRequest("User alread exists");
            }
            try 
            {
                string userId = await oktaMiddleware.AddUser(user);
                var newUser = new User(userId, user.FirstName, user.LastName, user.Email);
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = newUser.UserID }, new UserDTO(newUser));
            }
            catch ( NoIdForCreatedUserException ) 
            {
                return StatusCode(500);
            }
        }


        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Select( u => new UserDTO(u) ).ToListAsync();
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDTO(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(string id, UserDTO user)
        {

            var actualUser = await _context.Users.FindAsync(id);
            var issuerId = User.FindFirstValue("uid");
            if ( issuerId != actualUser.UserID) 
            {
                return BadRequest("Only user by itself could modify his profile");
            }
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<List<FilmDTO>>> GetUserReviews(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            //_context.Entry(user).Collection(u => u.AuthoredReviews).Load();
            var reviews = await Task.FromResult(user
                                                .AuthoredReviews
                                                .Select(r => new FilmDTO(r))
                                                .ToList());
            return reviews;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var issuerId = User.FindFirstValue("uid");
            if ( issuerId != user.UserID) 
            {
                return BadRequest("Only user can remove his profile");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new UserDTO(user);
        }

        private bool UserExists(UserDTO user)
        {
            return _context.Users.Any(e => e.UserID == user.ID || e.Email == user.Email );
        }
    }
}
