using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.DAL;
using FilmApi.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;


namespace FilmApi.Controllers
{
    public static class LinqExtensions 
    {
        private static PropertyInfo GetPropertyInfo(Type type, string name) 
        {
            var properties = type.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("name");
            return matchedProperty;
        }
        private static LambdaExpression GetOrderExpression(Type type, PropertyInfo info) 
        {
            var paramExpression = Expression.Parameter(type);
            var propAcces = Expression.PropertyOrField(paramExpression, info.Name);
            var expression = Expression.Lambda(propAcces, paramExpression);
            return expression;
        }
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name) 
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var propAccess = GetOrderExpression(typeof(T), propInfo);
            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, propAccess.Compile() });
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var access = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, access });
        }

        public static IEnumerable<T> OrderBy<T> (this IEnumerable<T> query, string [] names) 
        {
            foreach (var name in names)
            {
                query = query.OrderBy(name);
            }
            return query;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string[] names) 
        {
            foreach (var name in names) 
            {
                query = query.OrderBy(name);
            }
            return query;
        }
    }
    [Route("[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly Context _context;

        public FilmsController(Context context)
        {
            _context = context;
        }

        // GET: api/Films
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
            catch (ArgumentException ) 
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
        // GET: api/Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetFilm(long id)
        {
            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                return NotFound();
            }
            return await Task.FromResult(new
            {
                Film = new FilmDTO(film),
                Comments = film.Comments.Select(c => new CommentDTO(c)).ToList()
            });
        }

        // PUT: api/Films/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(long id, Film film)
        {
            if (id != film.FilmID)
            {
                return BadRequest();
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

        // POST: api/Films
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(Film film)
        {
            try 
            {
                if (FilmExists(film))
                    return BadRequest();
                _context.Films.Add(film);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) 
            {
                // Change on more verbose
                return BadRequest();
            }
            return CreatedAtAction("GetFilm", new { id = film.FilmID }, film);
        }

        // DELETE: api/Films/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FilmDTO>> DeleteFilm(long id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
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
