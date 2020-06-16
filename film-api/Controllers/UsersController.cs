using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.DAL;
using FilmApi.Models;

namespace FilmApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;

        public UsersController(Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
       /// [Authorize]
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
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
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
        public async Task<ActionResult<List<FilmDTO>>> GetUserReviews(long id)
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

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (UserExists(user))
                // Change on more verbose error code
                return BadRequest();
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }

        [HttpPost("signin")]
        public ActionResult SignIn() 
        {
            return Ok("Redirected");
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new UserDTO(user);
        }

        private bool UserExists(User user)
        {
            return _context.Users.Any(e => e.UserID == user.UserID || e.Username == user.Username || e.Email == user.Email);
        }
    }
}
