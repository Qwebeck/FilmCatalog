﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.DAL;
using FilmApi.Models;
using FilmApi.Utils;
using System.Security.Claims;
namespace FilmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly Context _context;

        public CommentsController(Context context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments()
        {
            return await _context.Comments.Select( c => new CommentDTO(c)).ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetComment(long id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return new CommentDTO(comment);
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutComment(long id, Comment comment)
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
            _context.Entry(comment).State = EntityState.Modified;

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

        // POST: api/Comments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentDTO>> PostComment([FromBody] CommentDTO comment)
        {
            var user = User.FindFirstValue("uid");
            var author = await _context.Users.FindAsync(user);
            var newComment = new Comment(comment) { UserID = user };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetComment", new { id = newComment.CommentID }, new CommentDTO(newComment) { Author = $"{author.FirstName} {author.LastName}"});
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
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
