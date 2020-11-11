using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonQ.DTO;
using AnonQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnonQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly QuestionContext _context;

        public CommentController(QuestionContext context)
        {
            _context = context;
        }

        // GET: api/Polls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentss()
        {
            return await _context.Comments
               .Select(x => CommentToDTO(x))
               .ToListAsync();
        }

        // GET: api/Polls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetComment(int id)
        {
            var todoItem = await _context.Comments.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return CommentToDTO(todoItem);
        }



        // PUT: api/Polls/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentDTO CommentDTO)
        {
            if (id != CommentDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.Comments.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.QuestionId = CommentDTO.QuestionId;
            todoItem.Text = CommentDTO.Text;
            todoItem.Votes = CommentDTO.Votes;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CommentExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPut("{id}/UpdateVotes")]
        public async Task<IActionResult> PutVotes(int id, PollsDTO pollsDTO)
        {
            if (id != pollsDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.Polls.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.QuestionId = pollsDTO.QuestionId;
            todoItem.Poll = pollsDTO.Poll;
            todoItem.Votes = pollsDTO.Votes + 1;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CommentExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }


        // POST: api/Polls
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> PostComment(Comment CommentDTO)
        {

            var todoItem = new Comment
            {
                //  QuestionId = pollsDTO.QuestionId,
                Text = CommentDTO.Text
                //  Votes = pollsDTO.Votes
            };

            _context.Comments.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetComment),
                new { id = todoItem.Id },
                CommentToDTO(todoItem));
        }

        // DELETE: api/Polls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommentDTO>> DeleteComment(int id)
        {
            var todoItem = await _context.Comments.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }

        public static CommentDTO CommentToDTO(Comment todoItem) =>
      new CommentDTO
      {
          Id = todoItem.Id,
          QuestionId = todoItem.QuestionId,
          Text = todoItem.Text,
          Votes = todoItem.Votes
      };
    }
}
