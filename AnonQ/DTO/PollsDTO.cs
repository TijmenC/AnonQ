using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonQ.DTO
{
    public class PollsDTO
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Poll { get; set; }
        public int Votes { get; set; }
    }
}
