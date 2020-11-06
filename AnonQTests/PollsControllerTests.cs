using AnonQ;
using FluentAssertions;
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
            var response = await _client.GetAsync("api/Polls/7");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Get_Request_Should_Return_Ok_One_Percentages()
        {
            var response = await _client.GetAsync("api/Polls/1/getPercentages");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
