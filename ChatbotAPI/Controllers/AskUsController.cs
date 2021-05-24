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
using ChatbotAPI.Services;

namespace ChatbotAPI.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AskUsController : ControllerBase {
        private readonly ChatbotAPIContext _context;
        private readonly IEmailService _emailService;

        public AskUsController(ChatbotAPIContext context, IEmailService emailService) {
            _context = context;
            _emailService = emailService;
        }

        [Authorize(new string[] { "Admin", "Staff" })]
        [HttpPost("{id}")]
        public async Task<IActionResult> Answer(int id, [FromBody] AnswerModel model) {
            var askUs = await _context.AskUs.FindAsync(id);

            if (askUs == null) {
                return NotFound();
            }

            string message = "";
            message += "<h4>Ask Us</h4>";
            message += "<p>Question:</p>";
            message += "<p>" + askUs.Text + "</p>";
            message += "<p>Sent by " + askUs.Firstname + " " + askUs.Lastname + "</p><br>";
            message += "<p>Staff's Answer:</p>";
            message += "<p>" + model.Text + "</p>";
            message += "<p>Answered by " + model.AnsweredBy + "</p><br>";
            message += "<p>Thank you for your question</p>";

            _emailService.Send(to: askUs.Email, subject: "Bilgi Staff's Answer", html: message);
            askUs.Answered = true;
            askUs.Answer = model.Text;
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

            return Ok(new {
                Message = "mail sent"
            });
        }

        // GET: api/AskUs
        [Authorize(new string[] { "Admin", "Staff"})]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AskUs>>> GetAskUs() {
            return await _context.AskUs.ToListAsync();
        }

        // GET: api/AskUs/5
        [Authorize(new string[] { "Admin", "Staff" })]
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
        [Authorize(new string[] { "Admin", "Staff" })]
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AskUs>> PostAskUs(AskUs askUs) {
            _context.AskUs.Add(askUs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAskUs", new { id = askUs.Id }, askUs);
        }

        // DELETE: api/AskUs/5
        [Authorize(new string[] { "Admin", "Staff" })]
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

        public class AnswerModel {
            public string AnsweredBy { get; set; }
            public string Text { get; set; }
        }
    }
}
