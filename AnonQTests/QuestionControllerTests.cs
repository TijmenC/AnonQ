using AnonQ;
using AnonQ.DTO;
using AnonQ.Models;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AnonQTests
{
    public class QuestionControllerTests : IClassFixture<AnonQFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly AnonQFactory<Startup> _factory;

        public QuestionControllerTests(
        AnonQFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        /*
        [Fact]
        public async Task Post_Succeed_Question()
        {
            var response = await _client.PostAsync("api/internships", new StringContent(JsonConvert.SerializeObject(new QuestionPollViewModel()
            {
                Question =
                {
                    Title = "title",
                    Description = "description",
                    Tag = "Relationship",
                    CommentsEnabled = true
                },
                Poll =
                {
                    new PollsDTO { Poll = "1"},
                    new PollsDTO { Poll = "1"}

                },
                Expiretime = 3
            }), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        */
        
        [Fact]
        public async Task Get_Request_Should_Return_Ok_One()
        {
            var response = await _client.GetAsync("api/Question/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_All()
        {
            var response = await _client.GetAsync("api/Question/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Wrong_ID()
        {
            var response = await _client.GetAsync("api/Questions/4");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_GetRandomQuestionId()
        {
            var response = await _client.GetAsync("api/Question/GetRandomQuestionId");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task Get_Request_Should_Return_Ok_QuestionAndPolls()
        {
            var response = await _client.GetAsync("api/Question/1/QuestionAndPolls");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        /*
        [Fact]
        public async Task Delete_Succeed_Question()
        {
            var response = await _client.DeleteAsync("api/Question/2");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        */



    }
}
