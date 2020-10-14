﻿using System;
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
            var todoItem = await _context.Polls.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return PollsToDTO(todoItem);
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

            var todoItem = await _context.Polls.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.QuestionId = pollsDTO.QuestionId;
            todoItem.Poll = pollsDTO.Poll;
            todoItem.Votes = pollsDTO.Votes;
        

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
            var todoItem = new Polls
            {
               QuestionId = pollsDTO.QuestionId,
               Poll = pollsDTO.Poll,
               Votes = pollsDTO.Votes
        };

            _context.Polls.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPoll),
                new { id = todoItem.Id },
                PollsToDTO(todoItem));
        }

        // DELETE: api/Polls/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PollsDTO>> DeletePoll(int id)
        {
            var todoItem = await _context.Polls.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Polls.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PollsExists(int id)
        {
            return _context.Polls.Any(e => e.Id == id);
        }

        private static PollsDTO PollsToDTO(Polls todoItem) =>
      new PollsDTO
      {
          Id = todoItem.Id,
          QuestionId = todoItem.QuestionId,
          Poll = todoItem.Poll,
          Votes = todoItem.Votes
      };
    }
}