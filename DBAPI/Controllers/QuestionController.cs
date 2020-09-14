using DBDataAcces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBAPI.Controllers
{
    public class QuestionController : ApiController
    {
        public IEnumerable<Question> GetQuestions()
        {
            using (AnonQEntities entities = new AnonQEntities())
            {
                return entities.Question.ToList();
            }
        }
        public Question GetQuestionID(int id)
        {
            using (AnonQEntities entities = new AnonQEntities())
            {
                return entities.Question.FirstOrDefault(e => e.id == id);
            }
        }
    }
}
