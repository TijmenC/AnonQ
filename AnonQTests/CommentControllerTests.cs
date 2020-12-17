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
    public class CommentControllerTests : IClassFixture<AnonQFactory<Startup>>
    {

        private readonly HttpClient _client;
        private readonly AnonQFactory<Startup> _factory;
        public CommentControllerTests(
        AnonQFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Request_Should_Return_Ok_One()
        {
            var response = await _client.GetAsync("api/Comment/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_All()
        {
            var response = await _client.GetAsync("api/comment/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Wrong_ID()
        {
            var response = await _client.GetAsync("api/comment/7");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_One_GetAllCommentsID()
        {
            var response = await _client.GetAsync("api/Comment/1/GetAllCommentsID");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Delete_Succeed_Comment()
        {
            var response = await _client.DeleteAsync("api/Comment/1");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task Post_Succeed_Comment()
        {
            var response = await _client.PostAsync("api/Comment", new StringContent(JsonConvert.SerializeObject(new CommentDTO()
            {
                Id = 10,
                QuestionId = 1,
                Text = "Comment 10",
                Votes = 8
            }), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task Put_Succeed_Comment()
        {
            var response = await _client.PutAsync("api/Comment/1",  new StringContent(JsonConvert.SerializeObject(new CommentDTO()
            {
                Id = 1,
                QuestionId = 1,
                Text = "Comment changed",
                Votes = 5
            }), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
