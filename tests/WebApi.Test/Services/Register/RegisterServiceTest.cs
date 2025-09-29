using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Services.Register
{
    [Collection("Integration Tests")]
    public class RegisterServiceTest : BarberBossClassFixture
    {
        private const string METHOD = "api/Service";

        private readonly string _token;

        public RegisterServiceTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestServiceJsonBuilder.Build();

            var result = await DoPost(requestUri: METHOD, request: request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.Created);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("serviceType").GetInt32().Should().Be((int)request.ServiceType!);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]

        public async Task Error_ServiceType_Empty(string culture)
        {
            var request = RequestServiceJsonBuilder.Build();
            request.ServiceType = null;

            var result = await DoPost(requestUri: METHOD, request: request, token: _token, culture: culture);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_TYPE_REQUIRED", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
