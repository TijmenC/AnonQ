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
    public class DeletePollsJob : IJob
    {
        private readonly ILogger<DeleteQuestionJob> _logger;
        private readonly IServiceProvider _provider;
        public DeletePollsJob(IServiceProvider provider, ILogger<DeleteQuestionJob> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<QuestionContext>();
                var overDueQuestionsId = dbContext.Questions.Where(p => p.DeletionTime < DateTime.UtcNow).Select(p => p.Id).ToArray();
                if (overDueQuestionsId != null)
                {
                    foreach (var questionid in overDueQuestionsId)
                    {
                        var deletionPolls = dbContext.Polls.Find(questionid);
                        if (deletionPolls != null)
                        {
                            dbContext.Remove(deletionPolls);
                            _logger.LogInformation("Deleted Polls");
                            dbContext.SaveChanges();
                        }
                    }
        
                }
                else
                {
                    _logger.LogInformation("Polls not found");
                }

            }

            return Task.CompletedTask;
        }
    }
}
