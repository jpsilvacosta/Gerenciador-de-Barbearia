using BarberBoss.Exception;
using FluentAssertions;
using Microsoft.OpenApi.Services;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using System.Globalization;

namespace WebApi.Test.Services.Delete
{
    public class DeleteServiceTest : BarberBossClassFixture
    {
        private const string METHOD = "api/Service";
        private readonly string _token;
        private readonly long _serviceId;

        public DeleteServiceTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _serviceId = webApplicationFactory.Service_MemberTeam.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(requestUri: $"{METHOD}/{_serviceId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);

            result = await DoGet(requestUri: $"{METHOD}/{_serviceId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Service_Not_Found(string culture)
        {
            var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_NOT_FOUND", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));


        }
    }
}
