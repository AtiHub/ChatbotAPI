using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Data;
using ChatbotAPI.Models;
using ChatbotAPI.DbFill;
using AuthorizeAttribute = ChatbotAPI.Helpers.AuthorizeAttribute;
using Microsoft.AspNetCore.Authorization;

namespace ChatbotAPI.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase {
        private readonly ChatbotAPIContext _context;

        public QuestionsController(ChatbotAPIContext context) {
            _context = context;
        }

        // GET: api/Questions
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestion() {
            return await _context.Question.ToListAsync();
        }

        // GET: api/Questions/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id) {
            var question = await _context.Question.FindAsync(id);

            if (question == null) {
                return NotFound();
            }

            return question;
        }

        [AllowAnonymous]
        [HttpGet("byCategoryId/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestionsByCategoryId(int categoryId) {
            var questions = await _context.Question.Where(question => question.CategoryId == categoryId).ToListAsync();
            if (questions == null) return NotFound();
            return questions;
        }

        // PUT: api/Questions/5
        [Authorize(new string[] { "Admin" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question) {
            if (id != question.Id) {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!QuestionExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Questions
        [Authorize(new string[] { "Admin" })]
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question) {
            _context.Question.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [Authorize(new string[] { "Admin" })]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Question>> DeleteQuestion(int id) {
            var question = await _context.Question.FindAsync(id);
            if (question == null) {
                return NotFound();
            }

            _context.Question.Remove(question);
            await _context.SaveChangesAsync();

            return question;
        }

        private bool QuestionExists(int id) {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
