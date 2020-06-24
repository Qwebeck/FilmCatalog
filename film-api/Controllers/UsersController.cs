
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

namespace FilmApi.Controllers
{
    [Route("[controller]")]
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
            
         

            //var client = new HttpClient();

            //Debug.WriteLine(content);
            //var message = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Post,
            //    RequestUri = new Uri(authorityUrl),
            //    Headers =
            //    {
            //        { HttpRequestHeader.Authorization.ToString(), $"SSWS {oktaToken}"},
            //    },
            //    Content = new StringContent(content, Encoding.UTF8, "application/json")
            //};
            //var response = await client.SendAsync(message);
            //var contents = await response.Content.ReadAsStringAsync();
            //if ( response.StatusCode == HttpStatusCode.OK ) 
            //{
            //    var matches = Regex.Match(contents, "\\\"id\\\":.?\\\"(?<id>.+?)\\\",");
            //    string id = matches.Groups["id"].Value;
            //    var newUser = new User(id, user.FirstName, user.LastName, user.Email );
            //    _context.Users.Add(newUser);
            //    await _context.SaveChangesAsync();
            //    return CreatedAtAction("GetUser", new { id = newUser.UserID }, new UserDTO(newUser));
            //}
            //else
            //{
            //    return BadRequest(contents);
            //}


            //var request = WebRequest.Create(authorityUrl);
            //request.Method = "POST";
            //request.Headers["Authorization"] = $"SSWS {oktaToken}";
            //request.Headers["Content-Type"] = "application/json";
            //request.Headers["Accept"] = "application/json";
            //request.Credentials = CredentialCache.DefaultCredentials;
            //string userData = JsonSerializer.Serialize(user, converterOptions);
            //Debug.WriteLine(userData);
            //Stream dataStream = request.GetRequestStream();
            //try
            //{
            //    await dataStream.WriteAsync(Encoding.ASCII.GetBytes(userData), 0, userData.Length);
            //    WebResponse response = await request.GetResponseAsync();
            //    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            //    response.Close();
            //}
            //catch ( WebException ex) 
            //{
            //    Debug.WriteLine("Bad request");
            //} 

            // var newUser = new User()
            //        "Authorization": `SSWS ${ this.token}`,
            //"Content-Type": "application/json",
            //"Accept": "application/json",
            //if (UserExists(user))
            //    // Change on more verbose error code
            //    return BadRequest();

            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            //return CreatedAtAction("GetUser", new { id = user.ID }, user);
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
            if (id != user.ID)
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
