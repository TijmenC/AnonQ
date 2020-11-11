using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonQ.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Votes { get; set; }
    }
}
