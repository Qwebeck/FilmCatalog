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
using Microsoft.AspNetCore.Http;

namespace FilmApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FilmsController : ControllerBase
    {
        private readonly Context _context;
        public FilmsController(Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Returns `number` of films starting from the offset 
        /// </summary>
        /// <param name="number">Number of films to fetch</param>
        /// <param name="offset">Number of films to skip</param>
        /// <response code="200">Collection of finded films</response>   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        /// Returns all existing genres
        /// </summary>
        /// <response code="200">Collection of existing genres</response>  
        [HttpGet("genres")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> GetGenres()
        {
            return await _context.Films
                        .Select( f => f.Genre )
                        .Distinct()
                        .ToListAsync();
        }

        /// <summary>
        /// Returns films, that matching given genres
        /// </summary>
        /// <param name="genres">Genres to which film should match</param>
        /// <param name="orderBy">Columns according to which result should be ordered</param>
        /// <param name="number">Number of films to return</param>
        /// <param name="offset">Number of films to skip</param>
        /// <returns></returns>
        /// <response code="200">Films with matching genres oredered by orderBy</response>  
        /// <response code="404">If no films with such genres exists</response>
        [HttpGet("findByGenres")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> FindByGenre([FromQuery(Name="genre")] string[] genres, [FromQuery] string[] orderBy, [FromQuery] int number=30, [FromQuery] int offset=0 ) 
        {
            orderBy = filterFields(orderBy);         
            if (orderBy.Length == 0)
                orderBy = new string[] { "Genre" };

            var films = await _context.Films
                              .Where(f => genres.Contains(f.Genre) || genres.Contains(f.Genre.ToLower()))
                              .Skip(offset)
                              .Take(number)
                              .OrderBy(orderBy)
                              .Select(f => new FilmDTO(f))
                              .ToListAsync();
             if (films.Count == 0)
                return NotFound($"No films that match any of requested genres: {string.Join(",", genres)}");
             return films;
            
        }

        /// <summary>
        /// Find films that have mark in specifed bounds
        /// </summary>
        /// <param name="lower">the lowest possible mark</param>
        /// <param name="upper">the highest possible mark</param>
        /// <param name="orderBy">columns to order by</param>
        /// <param name="number">Number of films to return</param>
        /// <param name="offset">Number of films to skip</param>
        /// <returns>Films which mark stays in bounds of lower and upper, orderedBy orderBy</returns>
        /// <response code="200">Films with mark in specified boundd oredered by orderBy</response>  
        /// <response code="404">If no films with mark in this bounds exists</response>
        [HttpGet("findByMarks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> FindByMark([FromQuery(Name = "lower")] int lower, [FromQuery(Name = "upper")] int upper, [FromQuery] string[] orderBy, [FromQuery] int number=30, [FromQuery] int offset=0)
        {
            if (lower > upper)
                return BadRequest("Lower mark couldn't be higher than upper");

            orderBy = filterFields(orderBy);
            ActionResult<IEnumerable<FilmDTO>> films;
            films = await _context.Films
                          .Where(f => f.Marks.Count != 0 
                                      && f.Marks.Average( m => m.MarkValue ) >= lower 
                                      && f.Marks.Average( m => m.MarkValue ) <= upper)
                          .Skip(offset)
                          .Take(number)
                          .OrderBy(orderBy)
                          .Select(f => new FilmDTO(f))
                          .ToListAsync();
            return films;
        }

        /// <summary>
        /// Returns films, title of witch contains provided subtitle
        /// </summary>
        /// <param name="title">String that should be in film title in order to return film</param>
        /// <param name="offset">Number of films to skip</param>
        /// <param name="number">Number of iflms to return</param>
        /// <returns></returns>       
        /// <response code="200">Films with matching subtitle oredered by orderBy</response>  
        /// <response code="404">If no films that match given subtitle</response>
        [HttpGet("findByTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> GetByTitle([FromQuery] string title,[FromQuery] int offset=0,[FromQuery] int number=30)
        {
            var films = await _context.Films
                              .Where(f => f.Title.Contains(title))
                              .Skip(offset)
                              .Take(number)
                              .ToListAsync();
            if (films.Count == 0) 
            {
                return NotFound($"No films with title similar to {title}");
            }
            return await Task.FromResult(films.Select( f => new FilmDTO(f)).ToList());
        }

        /// <summary>
        /// Returns description of film with id
        /// </summary>
        /// <param name="id">id of film that should be described</param>
        /// <param name="includeComments">if True, appends comments to film description</param>
        /// <returns></returns>
        /// <response code="200">Film with matching id</response>  
        /// <response code="404">If no film with id exists</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FilmDTO>> GetFilm(long id, [FromQuery] bool includeComments = true)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound($"No film with id {id}");
            }
            return await Task.FromResult(new FilmDTO(film, includeComments));
        }

        /// <summary>
        /// Return comments for given film
        /// </summary>
        /// <param name="id">Id of film, for which comments should be found</param>
        /// <returns></returns>
        /// <response code="200">Comments for films with given id</response>  
        /// <response code="404">If no comments for given film exists</response>
        [HttpGet("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CommentDTO>>> GetComments(long id) 
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null) 
            {
                return NotFound($"No film with id {id}");
            }
            return await Task.FromResult(film.Comments
                                         .Select(c => new CommentDTO(c))
                                         .ToList());
        }

        /// <summary>
        /// Returns user marks for film with filmID
        /// </summary>
        /// <param name="filmId"></param>
        /// <returns></returns>
        /// <response code="200">Marks for given film</response>  
        /// <response code="404">If no marks for given film exists</response>
        [HttpGet("{id}/marks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<MarkDTO>>> GetMarks(long filmId) 
        {
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                return NotFound();
            }
            return await Task.FromResult(film.Marks
                                         .Select(m => new MarkDTO(m))
                                         .ToList());
        }

        /// <summary>
        /// Returns images for film with given id
        /// </summary>
        /// <param name="id">id of film for which images should be fond</param>
        /// <returns></returns>
        /// <response code="200">Images for film with id</response>  
        /// <response code="404">If no images for film exists</response>
        [HttpGet("{id}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Image>>> GetImage(long id) 
        {
            var images = await _context.Images.Where(img => img.FilmID == id).ToListAsync();
            if ( images == null || images.Count == 0) 
            {
                return NotFound($"No images for film with id {id}");
            }
            return  images;
        }

        /// <summary>
        /// Updates film with given id
        /// </summary>
        /// <param name="id">id of film to update</param>
        /// <param name="film">new film description</param>
        /// <returns></returns>
        /// <response code="204">If film was successfully updated</response>  
        /// <response code="400">If no film that could be updated</response>
        /// <response code="404">If no marks for given film exists</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutFilm(long id, [FromBody] FilmDTO film)
        {
            var filmInDatabse = await _context.Films.FindAsync(id);
            if ( filmInDatabse == null )
            {
                return NotFound($"Film with id {id} doesn't exists");
            }
            if ( !( await User.IsAuthorizedForAction( filmInDatabse)) )
            {
                return BadRequest("User should be an author of review or administrator to edit it ");
            }
            var imagesForFilm = await _context.Images
                                        .Where( im => im.FilmID == filmInDatabse.FilmID )
                                        .ToListAsync();
            _context.Images.RemoveRange(imagesForFilm);
            await _context.SaveChangesAsync();
            if ( film.Images != null && film.Images.Length != 0 )
            {
                await AddImages(id, film.Images);              
            }

            filmInDatabse.Title = film.Title;
            filmInDatabse.Description = film.Description;
            filmInDatabse.Genre = film.Genre;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
              
            }
            return NoContent();
        }

        /// <summary>
        /// Adds new Film to the collection
        /// </summary>
        /// <param name="content">Description of film that should be added</param>
        /// <returns></returns>
        /// <response code="201">If film was succesfully created</response>  
        /// <response code="404">If film with similar title already exists</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Film>> PostFilm([FromBody] FilmDTO content)
        {
            string userID = User.FindFirstValue("uid");
            var film = new Film(content) { UserID = userID }; 
            if (FilmExists(film))
                 return BadRequest("FIlm Already exists");
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
            if(content.Images != null && content.Images.Length != 0) 
            {
                await AddImages(film.FilmID, content.Images);
            }
            return CreatedAtAction("GetFilm", new { id = film.FilmID }, film);
        }
        
        /// <summary>
        /// Deletes film with given id
        /// </summary>
        /// <param name="id">Id of film to delete</param>
        /// <returns></returns>
        /// <response code="200">If film was successfully deleted</response>  
        /// <response code="400">If user was not authorized for action</response>
        /// <respnse  code="404">If no film with given id was found</respnse>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// Adds new mark for film or replace previous 
        /// </summary>
        /// <param name="id">id of film to mark</param>
        /// <param name="newMark">new value of mark</param>
        /// <returns></returns>
        /// <response code="201">If new mark was created</response>  
        /// <response code="204">If mark was updated</response>
        /// <response code="404">If film id in content hadn't match id in url</response>
        [HttpPut("{id}/marks")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Mark>> MarkFilm(long id, [FromBody] MarkDTO newMark) 
        {
            if (id != newMark.FilmID)
                return BadRequest("Wrong mark format");
            var userID = User.FindFirstValue("uid");
            var userMark = await _context.Marks
                                  .Where(m => m.FilmID == id && m.UserID == userID)
                                  .FirstOrDefaultAsync();         
            ActionResult response; 
            if (userMark == null)
            {
                var mark = new Mark 
                {
                    MarkValue = newMark.Mark,
                    UserID = userID,
                    FilmID = newMark.FilmID        
                };
                _context.Marks.Add(mark);
                response = CreatedAtAction("MarkFilm", mark);
            }
            else 
            {
                _context.Entry(userMark).State = EntityState.Modified;
                userMark.MarkValue = newMark.Mark;
                response = NoContent();
            }
            await _context.SaveChangesAsync();
            return response;
        }
        private bool FilmExists(Film film)
        {
            return _context.Films.Any(e => e.FilmID == film.FilmID || e.Title == film.Title);
        }
        private async Task AddImages(long filmID, string[] images) 
        {
            var newImages = images.Select(image => new Image(filmID, image));
            _context.Images.AddRange(newImages);
            await _context.SaveChangesAsync();
        } 
        /// <summary>
        /// Select fields based that can be used to order films
        /// </summary>
        /// <param name="orderBy">fields given</param>
        /// <returns></returns>
        private string[] filterFields(string[] orderBy)
        {
            if (orderBy.Length == 0)
                return new string[] { "Genre" };
            var properties = typeof(FilmDTO).GetProperties()
                            .Select(p => p.Name);
            return orderBy.Where(f => properties.Contains(f)).ToArray();
        }
    }
}
