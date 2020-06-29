using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.DAL;
using FilmApi.Models;
using Microsoft.AspNetCore.Authorization;
using FilmApi.AuthorityProviders;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FilmApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;
        private readonly OktaMiddleware oktaMiddleware = new OktaMiddleware();

        public UsersController(Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Adds new user
        /// </summary>
        /// <param name="user">Description of user that should be added</param>
        /// <returns></returns>
        ///  <response code="201">If user was added</response>  
        /// <response code="500">If failure occured during obtaining id from authorizty</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        /// <summary>
        /// Returns all existing users
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Collection of users</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.Select( u => new UserDTO(u) ).ToListAsync();
        }


        /// <summary>
        /// Returns information about user with given id
        /// </summary>
        /// <param name="id">Id of user for which description should be found</param>
        /// <returns></returns>
        /// <response code="200">User with given id</response>
        /// <response code="404">If user wasn't found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return new UserDTO(user);
        }

        /// <summary>
        /// Updates information about user
        /// </summary>
        /// <param name="id">ID of user to update</param>
        /// <param name="user">New information about user</param>
        /// <returns></returns>
        /// <response code="400">If user is not authorized for this action</response>
        /// <response code="404">If no user with id exists</response>
        /// <response code="204">If user was successfully updated</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Returns film reviews authored by user
        /// </summary>
        /// <param name="id">Id of review author</param>
        /// <returns></returns>
        /// <response code="200">Collection of user reviews</response>
        /// <response code="204">If no user with given id exists</response>
        [HttpGet("{id}/reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FilmDTO>>> GetUserReviews(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            var reviews = await Task.FromResult(user
                                                .AuthoredReviews
                                                .Select(r => new FilmDTO(r))
                                                .ToList());
            return reviews;
        }
        /// <summary>
        /// Deletes user with given id
        /// </summary>
        /// <param name="id">Id of user to remove</param>
        /// <returns></returns>
        /// <response code="400">If user was not authorized for action</response>
        /// <response code="200">If user was successffuly removed</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
