using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;
using ChatbotAPI.Models;

namespace ChatbotAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AskUsController : ControllerBase {
        private readonly ChatbotAPIContext _context;

        public AskUsController(ChatbotAPIContext context) {
            _context = context;
        }

        // GET: api/AskUs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AskUs>>> GetAskUs() {
            return await _context.AskUs.ToListAsync();
        }

        // GET: api/AskUs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AskUs>> GetAskUs(int id) {
            var askUs = await _context.AskUs.FindAsync(id);

            if (askUs == null) {
                return NotFound();
            }

            return askUs;
        }

        // PUT: api/AskUs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAskUs(int id, AskUs askUs) {
            if (id != askUs.Id) {
                return BadRequest();
            }

            _context.Entry(askUs).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!AskUsExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AskUs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AskUs>> PostAskUs(AskUs askUs) {
            _context.AskUs.Add(askUs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAskUs", new { id = askUs.Id }, askUs);
        }

        // DELETE: api/AskUs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AskUs>> DeleteAskUs(int id) {
            var askUs = await _context.AskUs.FindAsync(id);
            if (askUs == null) {
                return NotFound();
            }

            _context.AskUs.Remove(askUs);
            await _context.SaveChangesAsync();

            return askUs;
        }

        private bool AskUsExists(int id) {
            return _context.AskUs.Any(e => e.Id == id);
        }
    }
}
