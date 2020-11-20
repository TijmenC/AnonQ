using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnonQ.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace AnonQJobs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            //Add DB
            services.AddDbContext<QuestionContext>
              (op => op.UseSqlServer(Configuration.GetConnectionString("AnonQDatabase")));
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add jobs

            services.AddSingleton<DeleteQuestionJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DeleteQuestionJob),
                cronExpression: "0 0/1 * * * ?")); // run every minute 


            services.AddHostedService<QuartzHostedService>();
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Jobs activated");
                });
            });
        }
    }
}
