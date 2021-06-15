using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecordingStudio.Dto;
using RecordingStudio.Interfaces;
using RecordingStudio.Models;

namespace RecordingStudio.Controllers
{
    [Route("api/studios")]
    [ApiController]
    public class StudioController : Controller
    {
        private readonly RecordingStudioDbContext _context;
        private readonly IAsyncRepository<Studio> _studioRepository;
        private readonly IMapper _mapper;

        public StudioController(RecordingStudioDbContext context, IMapper mapper,
            IAsyncRepository<Studio> studioRepository)
        {
            _studioRepository = studioRepository;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Studio
        [AllowAnonymous]
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
        public async Task<IActionResult> PutStudio(int id, [FromBody] StudioDto request)
        {
            var studio = await _studioRepository.GetByIdAsync(id);
            if (studio is null)
            {
                return NotFound();
            }

            try
            {
                studio.Address = request.Address;
                studio.Description= request.Description;
                studio.Price= request.Price;
                studio.Name= request.Name;
                await _studioRepository.UpdateAsync(studio);
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
        public async Task<ActionResult<Studio>> PostStudio([FromBody]Studio studio)
        {
            return await _studioRepository.AddAsync(studio);
        }

        // DELETE: api/Test/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudio(int id)
        {
            var studio = await _context.Studios.FindAsync(id);
            if (studio == null)
            {
                return NotFound();
            }

            await _studioRepository.DeleteAsync(studio);

            return NoContent();
        }

        private bool StudioExists(int id)
        {
            return _context.Studios.Any(e => e.Id == id);
        }
    }
}