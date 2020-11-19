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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<QuestionContext>
              (op => op.UseSqlServer(Configuration.GetConnectionString("AnonQDatabase")));
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add jobs
            //services.AddSingleton<HelloWorldJob>();
            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(HelloWorldJob),
            //    cronExpression: "0/5 * * * * ?")); // run every 5 seconds

            services.AddSingleton<DeletePollsJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DeletePollsJob),
                cronExpression: "0/5 * * * * ?")); // run every 5 seconds

            services.AddSingleton<DeleteQuestionJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DeleteQuestionJob),
                cronExpression: "0/25 * * * * ?")); // run every 25 seconds


            services.AddHostedService<QuartzHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
