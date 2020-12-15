using AnonQ.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonQJobs
{
    public class DeleteQuestionJob : IJob
    {
        private readonly ILogger<DeleteQuestionJob> _logger;
        private readonly IServiceProvider _provider;
        public DeleteQuestionJob(IServiceProvider provider, ILogger<DeleteQuestionJob> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<QuestionContext>();

                var overdueQuestions = dbContext.Questions.Where(p => p.DeletionTime < DateTime.UtcNow).ToArray();

                var overDueQuestionsId = dbContext.Questions.Where(p => p.DeletionTime < DateTime.UtcNow).Select(p => p.Id).ToArray();
                if (overDueQuestionsId != null)
                {
                    foreach (var questionid in overDueQuestionsId)
                    {
                        var deletionPolls = dbContext.Polls.Where(p => p.QuestionId == questionid).ToArray();
                        if (deletionPolls != null)
                        {
                            dbContext.Polls.RemoveRange(deletionPolls);
                            _logger.LogInformation("Deleted Polls");
                        }
                        var deletionComments = dbContext.Comments.Where(p => p.QuestionId == questionid).ToArray();
                        if (deletionComments != null)
                        {
                            dbContext.Comments.RemoveRange(deletionComments);
                            _logger.LogInformation("Deleted Comments");
                        }
                    }

                }

                if (overdueQuestions != null)
                {
                    foreach (var question in overdueQuestions)
                    {
                        dbContext.Questions.Remove(question);
                        _logger.LogInformation("Deleted Question");
                    }
                }
                dbContext.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
