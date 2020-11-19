using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonQ.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Tag { get; set; }
        public bool CommentsEnabled { get; set; }
        public DateTime DeletionTime { get; set; }
    }
}
