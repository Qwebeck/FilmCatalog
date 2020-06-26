using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmApi.DAL;
using FilmApi.Models;
using FilmApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FilmApi.Controllers
{
    [Route("api/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly Context _context;
        public FilmsController(Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Finds all existing films
        /// </summary>
        /// <param name="number">number of films to fetch</param>
        /// <param name="offset">film from which search shoud be started</param>
        /// <returns>Descriptions of `number` first films </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> GetFilms([FromQuery] int number = 30, [FromQuery] int offset = 0)
        {
            return await Task.FromResult(
                _context.Films
                .Skip(offset)
                .Take(number)
                .Select(f => new FilmDTO(f))
                .ToList());
        }

        /// <summary>
        /// Returns collection of films, that match given genres
        /// </summary>
        /// <param name="genres">Genres to match</param>
        /// <param name="order_by">Columns by which result will sorted</param>
        /// <param name="order">Ascending or descending order</param>
        /// <returns></returns>
        [HttpGet("findByGenres")]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> FindByGenre([FromQuery(Name="genre")] string[] genres, [FromQuery] string[] orderBy ) 
        {


            orderBy = filterFields(orderBy);
            
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
            catch ( ArgumentException ) 
            {
                return BadRequest("Cannot order by given field");
            }
            return films;
        }

        /// <summary>
        /// Find films that have mark in specifed bounds
        /// </summary>
        /// <param name="lower">the lowest possible mark</param>
        /// <param name="upper">the highest possible mark</param>
        /// <param name="orderBy">columns to order by</param>
        /// <returns>Films which mark stays in bounds of lower and upper, orderedBy orderBy</returns>
        [HttpGet("findByMarks")]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> FindByMark([FromQuery(Name = "lower")] int lower, [FromQuery(Name = "upper")] int upper, [FromQuery] string[] orderBy)
        {
            if (lower > upper)
                return BadRequest();

            orderBy = filterFields(orderBy);
            ActionResult<IEnumerable<FilmDTO>> films;
            try
            {
                films = await _context.Films
                                .Where(f => f.Marks.Count != 0 
                                       && f.Marks.Average( m => m.MarkValue ) >= lower 
                                       && f.Marks.Average( m => m.MarkValue ) <= upper)
                                .OrderBy(orderBy)
                                .Select(f => new FilmDTO(f))
                                .ToListAsync();
            }
            catch (ArgumentException)
            {
                return BadRequest("Cannot order by given fields");
            }
            return films;
        }
        
        /// <summary>
        /// Return films with matching title
        /// </summary>
        /// <param name="title">Substring of film title </param>
        /// <returns>List of matching films</returns>
        [HttpGet("findByTitle")]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> GetByTitle([FromQuery] string title)
        {
            var films = await _context.Films
                              .Where(f => f.Title.Contains(title))
                              .ToListAsync();
            if (films.Count == 0) 
            {
                return NotFound();
            }
            return await Task.FromResult(films.Select( f => new FilmDTO(f)).ToList());
        }  

        /// <summary>
        /// Return text information about film with id
        /// </summary>
        /// <param name="id">id of film to return</param>
        /// <returns>Description of film without image</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDTO>> GetFilm(long id, [FromQuery] bool includeComments = true)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return await Task.FromResult(new FilmDTO(film, includeComments));
        }

        /// <summary>
        /// Return comments for given film
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Return user marks for film with filmID
        /// </summary>
        /// <param name="filmId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Image of film with given id
        /// </summary>
        /// <param name="id">id of film for which image should be fond</param>
        /// <returns></returns>
        [HttpGet("{id}/image")]
        public async Task<ActionResult<Image>> GetImage(long id) 
        {
            var image = await _context.Images.Where(img => img.FilmID == id).FirstOrDefaultAsync();
            if ( image == null ) 
            {
                return NotFound();
            }
            return  image;
        }

        /// <summary>
        /// Updates film with given id
        /// </summary>
        /// <param name="id">id of film to update</param>
        /// <param name="film">new film description</param>
        /// <returns></returns>
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
                return BadRequest("User should be an author of review or administrator to edit it ");
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

        /// <summary>
        /// Adds new Film to the database
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Film>> PostFilm([FromBody] FilmDTO content)
        {
            string userID = User.FindFirstValue("uid");
            var film = new Film 
            {
            Title=content.Title, 
            Description=content.Description,
            Genre=content.Genre, 
            Director=content.Director, 
            UserID=userID
            };
            
            if (FilmExists(film))
                 return BadRequest();
            _context.Films.Add(film);
            await _context.SaveChangesAsync();

            var image = new Image
            {
                FilmID = film.FilmID,
                Data = content.Image
            };
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilm", new { id = film.FilmID }, film);
        }
        
        /// <summary>
        /// Deletes film with given id
        /// </summary>
        /// <param name="id">Id of film to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<FilmDTO>> DeleteFilm( long id )
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

      

        /// <summary>
        /// Adds new mark for film
        /// </summary>
        /// <param name="id">id of film to mark</param>
        /// <param name="newMark">new value of mark</param>
        /// <returns></returns>
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

     

        private bool FilmExists(Film film)
        {
            return _context.Films.Any(e => e.FilmID == film.FilmID || e.Title == film.Title);
        }

        /// <summary>
        /// Select fields based that can be used to order films
        /// </summary>
        /// <param name="orderBy">fields given</param>
        /// <returns></returns>
        private string[] filterFields(string[] orderBy)
        {
            if (orderBy.Length == 0)
                return new string[] { "Genres " };
            var properties = typeof(FilmDTO).GetProperties()
                            .Select(p => p.Name);
            return orderBy.Where(f => properties.Contains(f)).ToArray();
        }
    }
}
