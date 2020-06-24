using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmApi.DAL;
using FilmApi.Models;
using FilmApi.Utils;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FilmApi.Controllers
{
    [Route("[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly Context _context;
        private string ImageLocation = $"{Environment.CurrentDirectory}\\Data\\Images";
        private Random random = new Random();

        public FilmsController(Context context)
        {
            _context = context;
        }

        // GET: Films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> GetFilms()
        {
            return await Task.FromResult(_context.Films.Select(f => new FilmDTO(f)).ToList());
        }

        /// <summary>
        /// Returns collection of films, that match given genres
        /// </summary>
        /// <param name="genres">Genres to match</param>
        /// <param name="order_by">Columns by which result will sorted</param>
        /// <param name="order">Ascending or descending order</param>
        /// <returns></returns>
        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> FindByGenre([FromQuery(Name="genre")] string[] genres, [FromQuery] string[] orderBy ) 
        {
            
            if (orderBy.Length == 0)
                orderBy = new string[] { "Genre" };
            ActionResult<IEnumerable<FilmDTO>> films;
            try 
            {
                films = await _context.Films
                                .Where(f => genres.Contains(f.Genre) || genres.Contains(f.Genre.ToLower()))
                                .OrderBy(orderBy)
                                .Select(f => new FilmDTO(f))
                                .ToListAsync();
            }
            catch (ArgumentException ex) 
            {
                // TODO bad field to order by
                return BadRequest();
            }
            return films;
        }

        [HttpGet("marks")]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> FindByMark([FromQuery(Name = "lower")] int lower, [FromQuery(Name = "upper")] int upper, [FromQuery] string[] orderBy)
        {
            if (lower > upper)
                return BadRequest();
            if (orderBy.Length == 0)
                orderBy = new string[] { "Genre" };
            ActionResult<IEnumerable<FilmDTO>> films;
            try
            {
                films = await _context.Films
                                .Where(f => f.Marks.Count != 0 && f.Marks.Average( m => m.MarkValue ) >= lower && f.Marks.Average(m => m.MarkValue) <= upper)
                                .OrderBy(orderBy)
                                .Select(f => new FilmDTO(f))
                                .ToListAsync();
            }
            catch (ArgumentException)
            {
                // TODO bad field to order by
                return BadRequest();
            }
            return films;
        }
        // GET: Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetFilm(long id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            return await Task.FromResult(new FilmDTO(film));
        }

        // PUT: Films/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFilm(long id, Film film)
        {
            if (id != film.FilmID)
            {
                return BadRequest();
            }

            var filmInDatabse = await _context.Films.FindAsync(id);
            if ( !( await User.IsAuthorizedForAction( filmInDatabse)) )
            {
                return BadRequest("User should be author of review or administrator to edit it ");
            }

            _context.Entry(film).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(film))
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

        private string SaveImage(string? dataUri)
        {
            if ( dataUri == "" || dataUri == null ) return "";
            var matches = System.Text.RegularExpressions.Regex.Match(dataUri, @"data:image/(?<type>.+?);(?<base>.+?),(?<data>.+)");
            var data = Convert.FromBase64String(matches.Groups["data"].Value);
            var type = matches.Groups["type"].Value;
            var prefix = DateTime.Now.ToString("dmM");
            var path = $"{ImageLocation}\\{prefix}{random.Next()}.{type}";
            if (! System.IO.File.Exists(path))
            {
                Console.WriteLine(path);
                System.IO.File.WriteAllBytes( path, data);
            }
            else
            {
                throw new Exception("File already exists");
            }
            return path;
        }

        // POST: Films
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Film>> PostFilm([FromBody] FilmDTO content)
        {
            string userID = User.FindFirstValue("uid");
            var path = SaveImage(content.Image);
            var film = new Film {
            Title=content.Title, 
            Description=content.Description,
            Genre=content.Genre, 
            Director=content.Director, 
            ImagePath=path,
            UserID=userID};
            // var film = new Film(requestBody, path); 
            try 
            {
                if (FilmExists(film))
                    return BadRequest();
                _context.Films.Add(film);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            {
                // Change on more verbose
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            return CreatedAtAction("GetFilm", new { id = film.FilmID }, film);
        }

        // DELETE: Films/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<FilmDTO>> DeleteFilm(long id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            if (!( await User.IsAuthorizedForAction(film))) 
            {
                return BadRequest("User should be author of review or administrator to remove it ");
            }
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return new FilmDTO(film);
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult<List<CommentDTO>>> GetComments(long id) 
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
                return NotFound();
            return await Task.FromResult(film.Comments
                                            .Select(c => new CommentDTO(c))
                                            .ToList());
        }

        [HttpPut("{id}/marks")]
        [Authorize]
        public async Task<ActionResult<Mark>> MarkFilm(long id, Mark newMark) 
        {
            if (id != newMark.FilmID)
                return BadRequest();
            
            var userMark = await _context.Marks
                                       .Where(m => m.FilmID == id && m.UserID == newMark.UserID)
                                       .FirstOrDefaultAsync();
            ActionResult response;
            if (userMark == null)
            {
                _context.Marks.Add(newMark);
                response = CreatedAtAction("MarkFilm", userMark);
            }
            else 
            {
                _context.Entry(userMark).State = EntityState.Modified;
                userMark.MarkValue = newMark.MarkValue;
                response = NoContent();
            }
            await _context.SaveChangesAsync();

            return response;
        }

        [HttpGet("{id}/marks")]
        public async Task<ActionResult<List<MarkDTO>>> GetMarks(long filmId) 
        {
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                return NotFound();
            }
            _context.Entry(film).Collection(f => f.Marks).Load();
            return await Task.FromResult(film.Marks
                                    .Select(m => new MarkDTO(m))
                                    .ToList());
        }
        private bool FilmExists(Film film)
        {
            return _context.Films.Any(e => e.FilmID == film.FilmID || e.Title == film.Title);
        }
    }
}
