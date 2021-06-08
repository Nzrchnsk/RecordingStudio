using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingStudio.Models;

namespace RecordingStudio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioController : Controller
    {
        private readonly RecordingStudioDbContext _context;

        public StudioController(RecordingStudioDbContext context)
        {
            _context = context;
        }

        // GET: api/Studio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Studio>>> GetStudios()
        {
            return await _context.Studios.ToListAsync();
        }

        // GET: api/Studio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Studio>> GetStudio(int id)
        {
            var studio = await _context.Studios.FindAsync(id);

            if (studio == null)
            {
                return NotFound();
            }

            return studio;
        }

        // PUT: api/Test/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudio(int id, Studio studio)
        {
            if (id != studio.Id)
            {
                return BadRequest();
            }

            _context.Entry(studio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudioExists(id))
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

        // POST: api/Test
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Studio>> PostStudio(Studio studio)
        {
            _context.Studios.Add(studio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudio",
                new
                {
                    id = studio.Id
                },
                studio);
        }

        // DELETE: api/Test/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudio(int id)
        {
            var studio = await _context.Studios.FindAsync(id);
            if (studio == null)
            {
                return NotFound();
            }

            _context.Studios.Remove(studio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudioExists(int id)
        {
            return _context.Studios.Any(e => e.Id == id);
        }
    }
}