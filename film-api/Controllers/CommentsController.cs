using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.DAL;
using FilmApi.Models;
using FilmApi.Utils;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FilmApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly Context _context;

        public CommentsController(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Updates comment with given id
        /// </summary>
        /// <param name="id">ID of comment to update</param>
        /// <param name="comment">New comment content</param>
        /// <returns></returns>
        /// <response code="404">If comment wasn't found</response>
        /// <response code="400">If user is not authorized to access resource</response>
        /// <response code="204">If comment was successfully updated</response>   
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> PutComment(long id, [FromBody] CommentDTO comment)
        {
            if (id != comment.CommentID)
            {
                return BadRequest();
            }
            var realComment = await _context.Comments.FindAsync(id);
            if (!(await User.IsAuthorizedForAction(realComment))) 
            {
                return BadRequest("User should be an author or administrator to edit this comment");
            }            
            _context.Entry(new Comment(comment)).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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
        /// Returns the commment with given id
        /// </summary>
        /// <param name="id">ID of comment to update</param>
        /// <returns></returns>
        /// <response code="404">If comment wasn't found</response>
        /// <response code="204">If comment was successfully updated</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CommentDTO>> GetComment(long id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return new CommentDTO(comment);
        }

        /// <summary>
        /// Adds new comment to film
        /// </summary>
        /// <param name="comment">Comment that should be added</param>
        /// <returns></returns>
        /// <response code="204">If comment was successfully created</response>   
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CommentDTO>> PostComment([FromBody] CommentDTO comment)
        {
            var userID = User.FindFirstValue("uid");
            var author = await _context.Users.FindAsync(userID);
            var newComment = new Comment(comment) { UserID = userID };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetComment", new { id = newComment.CommentID }, new CommentDTO(newComment) { Author = $"{author.FirstName} {author.LastName}"});
        }

        /// <summary>
        /// Removes comment with given id
        /// </summary>
        /// <param name="id">Id of comment to remove</param>
        /// <returns></returns>
        /// <response code="404">If comment wasn't found</response>
        /// <response code="400">If user is not authorized to access resource</response>
        /// <response code="200">If comment was successfully removed</response>  
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<CommentDTO>> DeleteComment(long id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            if (!(await User.IsAuthorizedForAction(comment))) 
            {
                return BadRequest("User should be an administrator or comment author to edit it");
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return new CommentDTO(comment);
        }

        private bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.CommentID == id);
        }
    }
}
