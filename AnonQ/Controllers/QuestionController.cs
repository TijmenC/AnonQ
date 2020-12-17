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

        // GET: api/Question/5

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestions()
        {
            return await _context.Questions
                .Select(x => QuestionToDTO(x))
                .ToListAsync();
        }


        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return QuestionToDTO(question);
        }
        // GET: api/Question/GetRandomQuestionId
        [HttpGet("GetRandomQuestionId")]
        public int GetRandomQuestionID()
        {
            var question = _context.Questions
           .Select(p => p.Id)
           .ToArray();
            Random random = new Random();
            int randomid = question[random.Next(question.Length)];

            return randomid;
        }
        // GET: api/Question/QuestionAndPolls
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

            questionAndPoll.Question = questionDTO;
            questionAndPoll.Poll = pollsDTO;

            return questionAndPoll;

        }
    

        // PUT: api/Question/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, QuestionDTO questionDTO)
        {
            if (id != questionDTO.Id)
            {
                return BadRequest();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            question.Title = questionDTO.Title;
            question.Description = questionDTO.Description;
            question.Image = questionDTO.Image;
            question.Tag = questionDTO.Tag;
            question.CommentsEnabled = questionDTO.CommentsEnabled;
            question.DeletionTime = questionDTO.DeletionTime;

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
        // POST: api/Question
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("QuestionAndPoll")]
        public async Task<ActionResult<QuestionDTO>> CreateQuestionAndPolls(QuestionPollViewModel totalQuestion)
        {
            // TimeSpan addedHours = new TimeSpan(0, 0, 1, 0); (TimeSpan to test with)
            TimeSpan addedHours = new TimeSpan(0, totalQuestion.Expiretime, 0, 0);
            var expireTime = DateTime.UtcNow.Add(addedHours);

            var question = new Question
            {
                Id = totalQuestion.Question.Id,
                Title = totalQuestion.Question.Title,
                Description = totalQuestion.Question.Description,
                Image = totalQuestion.Question.Image,
                Tag = totalQuestion.Question.Tag,
                CommentsEnabled = totalQuestion.Question.CommentsEnabled,
                DeletionTime = expireTime
            };

         
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            var allPollsDTO = totalQuestion.Poll;
            List<Polls> allPolls = new List<Polls>();
            for (int i = 0; i < allPollsDTO.Count(); ++i)
            {
                var poll = new Polls
                {
                    Poll = totalQuestion.Poll[i].Poll,
                    QuestionId = question.Id
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
                new { id = question.Id },
                QuestionToDTO(question));
        }
        // POST: api/Question
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> CreateQuestion(QuestionDTO questionDTO)
        {
         
            var question = new Question
            {
                Id = questionDTO.Id,
                Title = questionDTO.Title,
                Description = questionDTO.Description,
                Image = questionDTO.Image,
                Tag = questionDTO.Tag,
                CommentsEnabled = questionDTO.CommentsEnabled,
                DeletionTime = questionDTO.DeletionTime
            };


            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

          
            return CreatedAtAction(
                nameof(GetQuestion),
                new { id = question.Id },
                QuestionToDTO(question));
        }



        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<QuestionDTO>> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
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
          CommentsEnabled = todoItem.CommentsEnabled,
          DeletionTime = todoItem.DeletionTime
      };
    }
}
