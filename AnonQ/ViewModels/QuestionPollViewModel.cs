using AnonQ.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonQ.Models
{
    public class QuestionPollViewModel
    {
        public QuestionDTO Question { get; set; }
        public List<PollsDTO> Poll { get; set; }
        public int Expiretime { get; set; }
    }
}
