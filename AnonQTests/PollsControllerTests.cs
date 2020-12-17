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
    public class PollsControllerTests : IClassFixture<AnonQFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly AnonQFactory<Startup> _factory;

        public PollsControllerTests(
        AnonQFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        /*
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
        */
        [Fact]
        public async Task Get_Request_Should_Return_Ok_One()
        {
            var response = await _client.GetAsync("api/Polls/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_All()
        {
            var response = await _client.GetAsync("api/Polls/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Wrong_ID()
        {
            var response = await _client.GetAsync("api/Polls/99");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_One_Percentages()
        {
            var response = await _client.GetAsync("api/Polls/1/getPercentages");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Delete_Succeed_Poll()
        {
            var response = await _client.DeleteAsync("api/Polls/2");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task Put_Succeed_Poll()
        {
            var response = await _client.PutAsync("api/Polls/1", new StringContent(JsonConvert.SerializeObject(new PollsDTO()
            {
                Id = 1,
                QuestionId = 1,
                Poll = "Poll changed",
                Votes = 2
            }), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task Put_Succeed_Poll_UpdateVotes()
        {
            var response = await _client.PutAsync("api/Polls/1/UpdateVotes", new StringContent(JsonConvert.SerializeObject(new PollsDTO()
            {
                Id = 1,
                QuestionId = 1,
                Poll = "Poll changed",
                Votes = 2
            }), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task Post_Succeed_Poll()
        {
            var response = await _client.PostAsync("api/Polls", new StringContent(JsonConvert.SerializeObject(new PollsDTO()
            {
                Id = 10,
                QuestionId = 1,
                Poll = "Poll 10",
                Votes = 8
            }), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
