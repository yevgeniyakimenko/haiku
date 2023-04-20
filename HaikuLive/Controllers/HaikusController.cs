using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaikuLive.Models;

namespace HaikuLive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HaikusController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public HaikusController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Haikus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Haiku>>> GetHaikus()
        {
          if (_context.Haikus == null)
          {
              return NotFound();
          }
            return await _context.Haikus.ToListAsync();
        }

        // GET: api/Haikus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Haiku>> GetHaiku(int id)
        {
          if (_context.Haikus == null)
          {
              return NotFound();
          }
            var haiku = await _context.Haikus.FindAsync(id);

            if (haiku == null)
            {
                return NotFound();
            }

            return haiku;
        }

        // PUT: api/Haikus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHaiku(int id, Haiku haiku)
        {
            if (id != haiku.Id)
            {
                return BadRequest();
            }

            _context.Entry(haiku).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HaikuExists(id))
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

        // POST: api/Haikus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Haiku>> PostHaiku(Haiku haiku)
        {
          if (_context.Haikus == null)
          {
              return Problem("Entity set 'DatabaseContext.Haikus'  is null.");
          }

            var httpClient = new System.Net.Http.HttpClient();
            var toxicityHost = Environment.GetEnvironmentVariable("TOXICITY_SERVER");
            var haikuText = haiku.Line1 + " " + haiku.Line2 + " " + haiku.Line3;
            var response = await httpClient.GetAsync($"{toxicityHost}/v1/classify?text={haikuText}");

            // returns a JSON object with an "isToxic" property
            var toxicityResponse = await response.Content.ReadAsStringAsync();
            if (toxicityResponse.Contains("true"))
            {
                return StatusCode(403, "Toxicity detected.");
            }

            _context.Haikus.Add(haiku);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHaiku", new { id = haiku.Id }, haiku);
        }

        // DELETE: api/Haikus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHaiku(int id)
        {
            if (_context.Haikus == null)
            {
                return NotFound();
            }
            var haiku = await _context.Haikus.FindAsync(id);
            if (haiku == null)
            {
                return NotFound();
            }

            _context.Haikus.Remove(haiku);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HaikuExists(int id)
        {
            return (_context.Haikus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
