using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;
using ChatbotAPI.Models;
using AuthorizeAttribute = ChatbotAPI.Helpers.AuthorizeAttribute;
using Microsoft.AspNetCore.Authorization;

namespace ChatbotAPI.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase {
        private readonly ChatbotAPIContext _context;

        public FAQsController(ChatbotAPIContext context) {
            _context = context;
        }

        // GET: api/FAQs
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetFAQ() {
            /*
            int[] ids = { 1, 2, 6, 8, 11, 13, 15, 21, 22, 33, 36, 38, 42, 44, 46, 52, 53, 71, 72, 75, 107, 108, 110, 157, 162, 166, 175, 181, 184, 188, 196 };
            foreach(int id in ids) {
                _context.FAQ.Add(new FAQ { QuestionId = id});
                await _context.SaveChangesAsync();
            }*/
            return await _context.FAQ
                .Include(faq => faq.Question).ThenInclude(q => q.Answer)
                .Include(faq => faq.Question).ThenInclude(q => q.Category)
                .ToListAsync();
        }

        // GET: api/FAQs/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<FAQ>> GetFAQ(int id) {
            var fAQ = await _context.FAQ.FindAsync(id);

            if (fAQ == null) {
                return NotFound();
            }

            return fAQ;
        }

        // PUT: api/FAQs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(new string[] { "Admin" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFAQ(int id, FAQ fAQ) {
            if (id != fAQ.Id) {
                return BadRequest();
            }

            _context.Entry(fAQ).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!FAQExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FAQs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(new string[] { "Admin" })]
        [HttpPost]
        public async Task<ActionResult<FAQ>> PostFAQ(FAQ fAQ) {
            _context.FAQ.Add(fAQ);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFAQ", new { id = fAQ.Id }, fAQ);
        }

        // DELETE: api/FAQs/5
        [Authorize(new string[] { "Admin" })]
        [HttpDelete("{id}")]
        public async Task<ActionResult<FAQ>> DeleteFAQ(int id) {
            var fAQ = await _context.FAQ.FindAsync(id);
            if (fAQ == null) {
                return NotFound();
            }

            _context.FAQ.Remove(fAQ);
            await _context.SaveChangesAsync();

            return fAQ;
        }

        private bool FAQExists(int id) {
            return _context.FAQ.Any(e => e.Id == id);
        }
    }
}
