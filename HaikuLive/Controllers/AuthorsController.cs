using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaikuLive.Models;
using HaikuLive.Hubs;

namespace HaikuLive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AuthorsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }
            return await _context.Authors.ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var httpClient = new System.Net.Http.HttpClient();
            var toxicityHost = Environment.GetEnvironmentVariable("TOXICITY_SERVER");
            var authorData = author.Name;
            var response = await httpClient.GetAsync($"{toxicityHost}/v1/classify?text={authorData}");

            var toxicityResponse = await response.Content.ReadAsStringAsync();
            if (toxicityResponse.Contains("true"))
            {
                return StatusCode(403, "Toxicity detected.");
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'DatabaseContext.Authors'  is null.");
            }

            var topics = await _context.Topics.ToListAsync();

            // if the author exists, return the author and all existing topics
            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == author.Name);
            if (existingAuthor != null)
            {
                var returnExistingData = new
                {
                    topics = topics,
                    author = existingAuthor
                };
                return Ok(returnExistingData);
            }

            var httpClient = new System.Net.Http.HttpClient();
            var toxicityHost = Environment.GetEnvironmentVariable("TOXICITY_SERVER");
            var authorData = author.Name;
            var response = await httpClient.GetAsync($"{toxicityHost}/v1/classify?text={authorData}");

            var toxicityResponse = await response.Content.ReadAsStringAsync();
            if (toxicityResponse.Contains("true"))
            {
                return StatusCode(403, "Toxicity detected.");
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var returnData = new
            {
                topics = topics,
                author = author
            };
            return Ok(returnData);
        }

        // GET: api/Authors/5/Haikus
        [HttpGet("{authorId}/Haikus")]
        public async Task<ActionResult<IEnumerable<Haiku>>> GetAuthorHaikus(int authorId)
        {
          /* if (_context.Authors == null)
          {
              return NotFound();
          }
            var author = await _context.Authors.FindAsync(authorId);

            if (author == null)
            {
                // return NotFound();
                return BadRequest();
            } */

            return await _context.Haikus.Where(h => h.AuthorId == authorId).ToListAsync();
        }

        // POST: api/Authors/5/Haikus
        [HttpPost("{authorId}/Haikus")]
        public async Task<ActionResult<Haiku>> PostAuthorHaiku(int authorId, Haiku haiku)
        {
          /* if (_context.Authors == null)
          {
              return Problem("Entity set 'DatabaseContext.Authors'  is null.");
          }
            var author = await _context.Authors.FindAsync(authorId);

            if (author == null)
            {
                return NotFound();
            } */

            var httpClient = new System.Net.Http.HttpClient();
            var toxicityHost = Environment.GetEnvironmentVariable("TOXICITY_SERVER");
            var haikuText = haiku.Line1 + " " + haiku.Line2 + " " + haiku.Line3;
            var response = await httpClient.GetAsync($"{toxicityHost}/v1/classify?text={haikuText}");

            // returns a JSON object with an "isToxic" property
            var toxicityResponse = await response.Content.ReadAsStringAsync();
            if (toxicityResponse.Contains("true"))
            {
                return StatusCode(403, "Your haiku is toxic. Please try again.");
            }

            haiku.AuthorId = authorId;
            _context.Haikus.Add(haiku);
            await _context.SaveChangesAsync();

            return haiku;
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
