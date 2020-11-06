using System;
using System.Collections.Generic;
using System.Text;
using AnonQ.Models;

namespace AnonQTests
{
    class Utilities
    {
        public static void InitializeDbForTests(QuestionContext db)
        {
            db.Questions.AddRange(GetSeedingQuestions());
            db.Polls.AddRange(GetSeedingPolls());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(QuestionContext db)
        {
            db.Questions.RemoveRange(db.Questions);
            db.Polls.RemoveRange(db.Polls);
            InitializeDbForTests(db);
        }

        public static List<Question> GetSeedingQuestions()
        {
            return new List<Question>()
    {
        new Question(){Id = 1, Title="Title", Description = "Description", Image="image.png", CommentsEnabled=true, Tag="Tag", Timer=5 },
        new Question(){Id = 2, Title="Title2", Description = "Description2", Image="image2.png", CommentsEnabled=true, Tag="Tag2", Timer=10 },
        new Question(){Id = 3, Title="Title3", Description = "Description3", Image="image3.png", CommentsEnabled=true, Tag="Tag3", Timer=15 }
    };
        }
        public static List<Polls> GetSeedingPolls()
        {
            return new List<Polls>()
    {
        new Polls(){Id = 1, QuestionId=1, Poll="Poll", Votes=5 },
        new Polls(){Id = 2, QuestionId=1, Poll="Poll2", Votes=10},
        new Polls(){Id = 3, QuestionId=2, Poll="Poll3", Votes=12 },
        new Polls(){Id = 4, QuestionId=2, Poll="Poll4", Votes=15 },
        new Polls(){Id = 5, QuestionId=3, Poll="Poll5", Votes=8 },
        new Polls(){Id = 6, QuestionId=3, Poll="Poll6", Votes=11 },
    };
        }
    }
}
