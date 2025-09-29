using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Services.Update
{
    [Collection("Integration Tests")]
    public class UpdateServiceTest : BarberBossClassFixture
    {
        private const string METHOD = "api/Service";

        private readonly string _token;
        private readonly long _serviceId;

        public UpdateServiceTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _serviceId = webApplicationFactory.Service_MemberTeam.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestServiceJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/{_serviceId}", request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]

        public async Task Error_ServiceType_Empty(string culture)
        {
            var request = RequestServiceJsonBuilder.Build();
            request.ServiceType = null;

            var result = await DoPut(requestUri: $"{METHOD}/{_serviceId}", request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_TYPE_REQUIRED", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]

        public async Task Error_Service_Not_Found(string culture)
        {
            var request = RequestServiceJsonBuilder.Build();

            var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_NOT_FOUND", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
