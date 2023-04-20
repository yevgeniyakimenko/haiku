using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaikuLive.Models;
using HaikuLive.Hubs;

namespace HaikuLive.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IHubContext<TopicHub> _hub;

    public TopicsController(DatabaseContext context, IHubContext<TopicHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    // GET: api/Topics
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Topic>>> GetTopic()
    {
        if (_context.Topics == null)
        {
            return NotFound();
        }
        return await _context.Topics.ToListAsync();
    }

    // GET: api/Topics/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Topic>> GetTopic(int id)
    {
        if (_context.Topics == null)
        {
            return NotFound();
        }
        var topic = await _context.Topics.FindAsync(id);

        if (topic == null)
        {
            return NotFound();
        }

        return topic;
    }

    // PUT: api/Topics/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTopic(int id, Topic topic)
    {
        if (id != topic.Id)
        {
            return BadRequest();
        }

        var httpClient = new System.Net.Http.HttpClient();
        var toxicityHost = Environment.GetEnvironmentVariable("TOXICITY_SERVER");
        var response = await httpClient.GetAsync($"{toxicityHost}/v1/classify?text={topic.Name}");

        var toxicityResponse = await response.Content.ReadAsStringAsync();
        if (toxicityResponse.Contains("true"))
        {
            return StatusCode(403, "The topic is toxic. Please try again.");
        }

        _context.Entry(topic).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TopicExists(id))
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

    // POST: api/Topics
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Topic>> PostTopic(Topic topic)
    {
        if (_context.Topics == null)
        {
            return Problem("Entity set 'DatabaseContext.Topic'  is null.");
        }

        var topicExists = _context.Topics.Any(t => t.Name == topic.Name);
        if (topicExists)
        {
            return Problem("Topic already exists.");
        }

        var httpClient = new System.Net.Http.HttpClient();
        var toxicityHost = Environment.GetEnvironmentVariable("TOXICITY_SERVER");
        var response = await httpClient.GetAsync($"{toxicityHost}/v1/classify?text={topic.Name}");

        var toxicityResponse = await response.Content.ReadAsStringAsync();
        if (toxicityResponse.Contains("true"))
        {
            return StatusCode(403, "The topic is toxic. Please try again.");
        }

        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTopic", new { id = topic.Id }, topic);
    }

    // GET: api/Topics/5/Haikus
    [HttpGet("{topicId}/Haikus")]
    public async Task<ActionResult<IEnumerable<Haiku>>> GetTopicHaikus(int topicId)
    {
        return await _context.Haikus.Where(h => h.TopicId == topicId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }

    // POST: api/Topics/5/Haikus
    [HttpPost("{topicId}/Haikus")]
    public async Task<ActionResult<Haiku>> PostTopicHaiku(int topicId, Haiku haiku)
    {
        var httpClient = new System.Net.Http.HttpClient();
        var toxicityHost = Environment.GetEnvironmentVariable("TOXICITY_SERVER");
        var haikuText = haiku.Line1 + " " + haiku.Line2 + " " + haiku.Line3;
        var response = await httpClient.GetAsync($"{toxicityHost}/v1/classify?text={haikuText}");

        var toxicityResponse = await response.Content.ReadAsStringAsync();
        if (toxicityResponse.Contains("true"))
        {
            return StatusCode(403, "Your haiku is toxic. Please try again.");
        }

        haiku.TopicId = topicId;
        _context.Haikus.Add(haiku);
        await _context.SaveChangesAsync();
        await _hub.Clients.Group(topicId.ToString()).SendAsync("ReceiveMessage", haiku);

        return haiku;
    }

    // DELETE: api/Topics/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        if (_context.Topics == null)
        {
            return NotFound();
        }
        var topic = await _context.Topics.FindAsync(id);
        if (topic == null)
        {
            return NotFound();
        }

        _context.Topics.Remove(topic);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TopicExists(int id)
    {
        return (_context.Topics?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
