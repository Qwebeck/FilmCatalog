using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.DAL;
using FilmApi.Models;

namespace film_api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly Context _context;

        public ImagesController(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns images for films with given id
        /// </summary>
        /// <param name="filmIDs">Ids for which images should be returned</param>
        /// <returns></returns>
        /// <response code="200">Images for films with given ids</response>  
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Image>>> GetImages([FromQuery(Name = "filmid")] long[] filmIDs)
        {

            return await _context.Images
                         .Where( i => filmIDs.Contains(i.FilmID))
                         .ToListAsync();
        }
    }
}
