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
    public class CategoriesController : ControllerBase {
        private readonly ChatbotAPIContext _context;

        public CategoriesController(ChatbotAPIContext context) {
            _context = context;
        }

        // GET: api/Categories
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory() {
            return await _context.Category.ToListAsync();
        }

        // GET: api/Categories/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id) {
            var category = await _context.Category.FindAsync(id);

            if (category == null) {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        [Authorize(new string[] { "Admin" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category) {
            if (id != category.Id) {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!CategoryExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        [Authorize(new string[] { "Admin" })]
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category) {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [Authorize(new string[] { "Admin" })]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id) {
            var category = await _context.Category.FindAsync(id);
            if (category == null) {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        private bool CategoryExists(int id) {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
