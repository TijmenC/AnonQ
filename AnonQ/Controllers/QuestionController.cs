using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnonQ.Models;
using AnonQ.DTO;

namespace AnonQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionContext _context;

        public QuestionController(QuestionContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems/5

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestions()
        {
            return await _context.Questions.ToListAsync();
        }
        

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(long id)
        {
            var todoItem = await _context.Questions.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(long id, QuestionDTO todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> PostQuestion(QuestionDTO todoItem)
        {
            _context.Questions.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestion), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestionDTO>> DeleteQuestion(long id)
        {
            var todoItem = await _context.Questions.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool QuestionExists(long id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
