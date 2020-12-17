using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnonQ.Models;
using AnonQ.DTO;
using System.Runtime.InteropServices;

namespace AnonQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly QuestionContext _context;

        public PollsController(QuestionContext context)
        {
            _context = context;
        }

        // GET: api/Polls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollsDTO>>> GetPolls()
        {
            return await _context.Polls
               .Select(x => PollsToDTO(x))
               .ToListAsync();
        }

        // GET: api/Polls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollsDTO>> GetPoll(int id)
        {
            var poll = await _context.Polls.FindAsync(id);

            if (poll == null)
            {
                return NotFound();
            }

            return PollsToDTO(poll);
        }

        // GET: api/Polls/5/getPercentages
        [HttpGet("{questionid}/getPercentages")]
        public async Task<List<PollPercentageViewModel>> GetPercentages(int questionid)
        {
            List<PollPercentageViewModel> allPollPercentages = new List<PollPercentageViewModel>();
            List<Polls> polls = await _context.Polls.Where(s => s.QuestionId == questionid).ToListAsync();
            int totalVotes =  _context.Polls.Where(s => s.QuestionId == questionid).Select(s => s.Votes).Distinct().Sum();
            foreach (var poll in polls)
            {
                int percentage = (int)Math.Round((double)100 * poll.Votes / totalVotes);
                var pollpercentage = PollsToPercentage(poll, percentage);
                allPollPercentages.Add(pollpercentage);
            }
            return allPollPercentages;
        }

   

        // PUT: api/Polls/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolls(int id, PollsDTO pollsDTO)
        {
            if (id != pollsDTO.Id)
            {
                return BadRequest();
            }

            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }

            poll.QuestionId = pollsDTO.QuestionId;
            poll.Poll = pollsDTO.Poll;
            poll.Votes = pollsDTO.Votes;
        

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PollsExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        // PUT: api/Polls/5/UpdateVotes
        [HttpPut("{id}/UpdateVotes")]
        public async Task<IActionResult> PutVotes(int id, PollsDTO pollsDTO)
        {
            if (id != pollsDTO.Id)
            {
                return BadRequest();
            }

            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }

            poll.QuestionId = pollsDTO.QuestionId;
            poll.Poll = pollsDTO.Poll;
            poll.Votes = pollsDTO.Votes +1;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PollsExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }


        // POST: api/Polls
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PollsDTO>> PostPolls(PollsDTO pollsDTO)
        {

            var poll = new Polls
            {           
               Poll = pollsDTO.Poll
            };

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPoll),
                new { id = poll.Id },
                PollsToDTO(poll));
        }

        // DELETE: api/Polls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PollsDTO>> DeletePoll(int id)
        {
            var poll = await _context.Polls.FindAsync(id);

            if (poll == null)
            {
                return NotFound();
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PollsExists(int id)
        {
            return _context.Polls.Any(e => e.Id == id);
        }

        public static PollsDTO PollsToDTO(Polls todoItem) =>
      new PollsDTO
      {
          Id = todoItem.Id,
          QuestionId = todoItem.QuestionId,
          Poll = todoItem.Poll,
          Votes = todoItem.Votes
      };
        public static PollPercentageViewModel PollsToPercentage(Polls todoItem, int percentage) =>
     new PollPercentageViewModel
     {
         Id = todoItem.Id,
         QuestionId = todoItem.QuestionId,
         Poll = todoItem.Poll,
         Votes = todoItem.Votes,
         Percentage =  percentage
     };
    }
}
