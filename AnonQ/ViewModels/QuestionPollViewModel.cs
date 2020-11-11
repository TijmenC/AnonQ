using AnonQ.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonQ.Models
{
    public class QuestionPollViewModel
    {
        public QuestionDTO question { get; set; }
        public List<PollsDTO> poll { get; set; }
    }
}
