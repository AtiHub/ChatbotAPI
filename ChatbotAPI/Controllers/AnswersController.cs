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
    public class AnswersController : ControllerBase {
        private readonly ChatbotAPIContext _context;

        public AnswersController(ChatbotAPIContext context) {
            _context = context;
        }

        // GET: api/Answers
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswer() {
            return await _context.Answer.ToListAsync();
        }

        // GET: api/Answers/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(int id) {
            var answer = await _context.Answer.FindAsync(id);

            if (answer == null) {
                return NotFound();
            }

            return answer;
        }

        // PUT: api/Answers/5
        [Authorize(new string[] { "Admin"})]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(int id, Answer answer) {
            if (id != answer.Id) {
                return BadRequest();
            }

            _context.Entry(answer).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!AnswerExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Answers
        [Authorize(new string[] { "Admin" })]
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer(Answer answer) {
            _context.Answer.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
        }

        // DELETE: api/Answers/5
        [Authorize(new string[] { "Admin" })]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Answer>> DeleteAnswer(int id) {
            var answer = await _context.Answer.FindAsync(id);
            if (answer == null) {
                return NotFound();
            }

            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();

            return answer;
        }

        private bool AnswerExists(int id) {
            return _context.Answer.Any(e => e.Id == id);
        }
    }
}
