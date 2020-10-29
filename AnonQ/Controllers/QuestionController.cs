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
            return await _context.Questions
                .Select(x => QuestionToDTO(x))
                .ToListAsync();
        }


        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(int id)
        {
            var todoItem = await _context.Questions.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return QuestionToDTO(todoItem);
        }

        [HttpGet("GetRandomQuestionId")]
        public int GetRandomQuestionID()
        {
            var todoItem = _context.Questions
           .Select(p => p.Id)
           .ToArray();
            Random random = new Random();
            int randomid = todoItem[random.Next(todoItem.Length)];

            return randomid;
        }

        [HttpGet("{id}/QuestionAndPolls")]
        public async Task<ActionResult<QuestionPollViewModel>> GetQuestionAndPolls(int id)
        {
            List<PollsDTO> pollsDTO = new List<PollsDTO>();
            QuestionPollViewModel questionAndPoll = new QuestionPollViewModel();

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            var questionDTO = QuestionToDTO(question);
            List<Polls> polls = await _context.Polls.Where(s => s.QuestionId == id).ToListAsync();
            foreach (var poll in polls)
            {
                pollsDTO.Add(PollsController.PollsToDTO(poll));
            }

            questionAndPoll.question = questionDTO;
            questionAndPoll.poll = pollsDTO;

            return questionAndPoll;

        }
        [HttpGet("GetQuestionIDByTitle/{title}")]
        public int GetQuestionIDByTitle(string title)
        {
            var todoItem = _context.Questions
            .Where(s => s.Title == title)
            .Select(s => s.Id).FirstOrDefault();

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(long id, QuestionDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.Questions.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Title = todoItemDTO.Title;
            todoItem.Description = todoItem.Description;
            todoItem.Image = todoItem.Image;
            todoItem.Tag = todoItem.Tag;
            todoItem.Timer = todoItem.Timer;
            todoItem.CommentsEnabled = todoItem.CommentsEnabled;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!QuestionExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> CreateQuestion(QuestionPollViewModel totalQuestion)
        {
            /*
            var todoItem = new Question
            {
                Title = questionDTO.Title,
                Description = questionDTO.Description,
                Image = questionDTO.Image,
                Tag = questionDTO.Tag,
                Timer = questionDTO.Timer,
                CommentsEnabled = questionDTO.CommentsEnabled
            };
            */

            var todoItem = new Question
            {
                Id = totalQuestion.question.Id,
                Title = totalQuestion.question.Title,
                Description = totalQuestion.question.Description,
                Image = totalQuestion.question.Image,
                Tag = totalQuestion.question.Tag,
                Timer = totalQuestion.question.Timer,
                CommentsEnabled = totalQuestion.question.CommentsEnabled
            };

         
            _context.Questions.Add(todoItem);
            await _context.SaveChangesAsync();

            var allPollsDTO = totalQuestion.poll;
            List<Polls> allPolls = new List<Polls>();
            for (int i = 0; i < allPollsDTO.Count(); ++i)
            {
                var poll = new Polls
                {
                    Poll = totalQuestion.poll[i].Poll,
                    QuestionId = todoItem.Id
                };
                allPolls.Add(poll);
            }

            foreach (var pollitem in allPolls)
            {
                _context.Polls.Add(pollitem);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(
                nameof(GetQuestion),
                new { id = todoItem.Id },
                QuestionToDTO(todoItem));
        }


        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(long id)
        {
            var todoItem = await _context.Questions.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(long id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
        private static QuestionDTO QuestionToDTO(Question todoItem) =>
      new QuestionDTO
      {
          Id = todoItem.Id,
          Title = todoItem.Title,
          Description = todoItem.Description,
          Image = todoItem.Image,
          Tag = todoItem.Tag,
          Timer = todoItem.Timer,
          CommentsEnabled = todoItem.CommentsEnabled
      };
    }
}
