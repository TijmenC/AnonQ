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
            db.Comments.AddRange(GetSeedingComments());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(QuestionContext db)
        {
            db.Questions.RemoveRange(db.Questions);
            db.Polls.RemoveRange(db.Polls);
            db.Comments.RemoveRange(db.Comments);
            InitializeDbForTests(db);
        }



        public static List<Question> GetSeedingQuestions()
        {
            return new List<Question>()
    {
        new Question(){Id = 1, Title="Title", Description = "Description", Image="image.png", CommentsEnabled=true, Tag="Tag", DeletionTime=new DateTime(2020, 12, 25) },
        new Question(){Id = 2, Title="Title2", Description = "Description2", Image="image2.png", CommentsEnabled=true, Tag="Tag2", DeletionTime=new DateTime(2020, 12, 26) },
        new Question(){Id = 3, Title="Title3", Description = "Description3", Image="image3.png", CommentsEnabled=true, Tag="Tag3", DeletionTime=new DateTime(2020, 12, 27) },
        new Question(){Id = 4, Title="Title4", Description = "Description4", Image="image4.png", CommentsEnabled=true, Tag="Tag4", DeletionTime=new DateTime(2020, 12, 28) }
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
        new Polls(){Id = 7, QuestionId=4, Poll="Poll7", Votes=16 },
        new Polls(){Id = 8, QuestionId=4, Poll="Poll8", Votes=12 }
    };
        }
        public static List<Comment> GetSeedingComments()
        {
            return new List<Comment>()
    {
        new Comment(){Id = 1, QuestionId=1, Text="Comment1", Votes=5 },
        new Comment(){Id = 2, QuestionId=1, Text="Comment2", Votes=10},
        new Comment(){Id = 3, QuestionId=2, Text="Comment3", Votes=12 },
        new Comment(){Id = 4, QuestionId=2, Text="Comment4", Votes=15 },
        new Comment(){Id = 5, QuestionId=3, Text="Comment5", Votes=8 },
        new Comment(){Id = 6, QuestionId=3, Text="Comment6", Votes=11 },
        new Comment(){Id = 7, QuestionId=4, Text="Comment7", Votes=8 },
        new Comment(){Id = 8, QuestionId=4, Text="Comment8", Votes=14 }
    };
        }
    }
}
