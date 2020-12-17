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
        [Fact]
        public async Task Get_Request_Should_Return_Ok_One()
        {
            var response = await _client.GetAsync("api/question/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_All()
        {
            var response = await _client.GetAsync("api/question/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Wrong_ID()
        {
            var response = await _client.GetAsync("api/question/8");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_GetRandomQuestionId()
        {
            var response = await _client.GetAsync("api/question/GetRandomQuestionId");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_QuestionAndPolls()
        {
            var response = await _client.GetAsync("api/question/1/QuestionAndPolls");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Delete_Succeed_Question()
        {
            var response = await _client.DeleteAsync("api/Question/2");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        [Fact]
        public async Task Post_Succeed_Question()
        {
            var response = await _client.PostAsync("api/Question", new StringContent(JsonConvert.SerializeObject(new QuestionDTO()
            {
                Id = 10,
                Title = "title",
                Description = "description",
                Tag = "Relationship",
                CommentsEnabled = true,
                Image = "image.png",
                DeletionTime = new DateTime(2020, 12, 25)
            }), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task Post_Succeed_QuestionAndPoll()
        {
            List<PollsDTO> polls = new List<PollsDTO>();
            var NewQuestion = new QuestionDTO();
            polls.Add(new PollsDTO() { Id = 11, QuestionId = 11, Poll = "Poll 1", Votes = 11 });
            polls.Add(new PollsDTO() { Id = 10, QuestionId = 11, Poll = "Poll 2", Votes = 12 });
            var vm = new QuestionPollViewModel();
            NewQuestion.Title = "title";
            NewQuestion.Description = "description";
            NewQuestion.CommentsEnabled = true;
            NewQuestion.Image = "image.png";
            NewQuestion.Tag = "Relationship";
            NewQuestion.DeletionTime = new DateTime(2020, 12, 25);
            vm.Poll = polls;
            vm.Question = NewQuestion;
            vm.Expiretime = 3;

            var response = await _client.PostAsync("api/Question/QuestionAndPoll", new StringContent(JsonConvert.SerializeObject(vm), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task Put_Succeed_Question()
        {
            var response = await _client.PutAsync("api/Question/3", new StringContent(JsonConvert.SerializeObject(new QuestionDTO()
            {
                Id = 3,
                Title = "title changed",
                Description = "description changed",
                Tag = "Relationship changed",
                CommentsEnabled = false,
                Image = "imagechanged.png",
                DeletionTime = new DateTime(2020, 12, 22)
            }), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
