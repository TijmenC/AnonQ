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

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments(int id)
        {
            return await _context.Comments
               .Select(x => CommentToDTO(x))
               .ToListAsync();
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return CommentToDTO(comment);
        }
        // GET: api/Comment/5/GetAllCommentsID
        [HttpGet("{id}/GetAllCommentsID")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetAllCommentsID(int id)
        {
            List<CommentDTO> commentsDTO = new List<CommentDTO>();
            List<Comment> comments = await _context.Comments.Where(s => s.QuestionId == id).ToListAsync();
            foreach (var comment in comments)
            {
                commentsDTO.Add(CommentToDTO(comment));
            }
            return commentsDTO;
        }



        // PUT: api/Comment/5
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


        // POST: api/Comment
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> PostComment(CommentDTO CommentDTO)
        {

            var comment = new Comment
            {
                QuestionId = CommentDTO.QuestionId,
                Text = CommentDTO.Text
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetComment),
                new { id = comment.Id },
                CommentToDTO(comment));
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommentDTO>> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
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
