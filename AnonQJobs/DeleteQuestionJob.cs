using AnonQ.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnonQJobs
{
    public class DeleteQuestionJob : IJob
    {
        private readonly IServiceProvider _provider;
        public DeleteQuestionJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<QuestionContext>();
                // fetch customers, send email, update DB
              

            }

            return Task.CompletedTask;
        }
    }
}
