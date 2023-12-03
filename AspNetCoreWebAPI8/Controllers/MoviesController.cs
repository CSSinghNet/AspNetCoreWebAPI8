using AspNetCoreWebAPI8.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebAPI8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _movieContext;
        public MoviesController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        // Get : api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_movieContext.Movies == null)
            {
                return NotFound();
            }
            return await _movieContext.Movies.ToListAsync();
        }

        // Get : api/Movies/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_movieContext.Movies is null)
            {
                return NotFound();
            }
            var movie = await _movieContext.Movies.FindAsync(id);
            if (movie is null)
            {
                return NotFound();
            }
            return movie;
        }

        // Post : api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _movieContext.Movies.Add(movie);
            await _movieContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // Put : api/Movies/2
        [HttpPut]
        public async Task<ActionResult<Movie>> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }
            _movieContext.Entry(movie).State = EntityState.Modified;
            try
            {
                await _movieContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool MovieExists(long id)
        {
            return (_movieContext.Movies?.Any(movie => movie.Id == id)).GetValueOrDefault();
        }

        // Delete : api/Movies/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            if (_movieContext.Movies is null)
            {
                return NotFound();
            }
            var movie= await _movieContext.Movies.FindAsync(id);
            if (movie is null)
            {
                return NotFound();
            }
          _movieContext.Movies.Remove(movie);
            await _movieContext.SaveChangesAsync();
            return NoContent();
        }


    }
}
