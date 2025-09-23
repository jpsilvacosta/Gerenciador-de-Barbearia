using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Services.GetById
{
    public class GetServiceByIdTest : BarberBossClassFixture
    {
        private const string METHOD = "api/Service";
        private readonly string _token;
        private readonly long _serviceId;

        public GetServiceByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _serviceId = webApplicationFactory.Service_MemberTeam.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoGet(requestUri: $"{METHOD}/{_serviceId}", token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("id").GetInt64().Should().Be(_serviceId);
            response.RootElement.GetProperty("description").GetString().Should().NotBeNull();
            response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
            response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);

            var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
            Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();

            var serviceType = response.RootElement.GetProperty("serviceType").GetInt32();
            Enum.IsDefined(typeof(ServiceType), serviceType).Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]

        public async Task Error_Service_Not_Found(string culture)
        {
            var invalidServiceId = long.MaxValue;

            var result = await DoGet(requestUri: $"{METHOD}/{invalidServiceId}", token: _token, culture);

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_NOT_FOUND", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));

        }
    }
}
