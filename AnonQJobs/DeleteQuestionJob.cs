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
                // fetch customers, send email, update DB
                var overdueQuestions = dbContext.Questions.Where(p => p.DeletionTime < DateTime.UtcNow).ToArray();
                if (overdueQuestions != null)
                {
                    foreach (var question in overdueQuestions)
                    {
                        dbContext.Questions.Remove(question);
                        _logger.LogInformation("Deleted Question");
                    }
                    dbContext.SaveChanges();
                }
                else
                {
                    _logger.LogInformation("Questions not found");
                }

            }

            return Task.CompletedTask;
        }
    }
}
