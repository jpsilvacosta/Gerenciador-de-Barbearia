using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Services.Reports
{
    public class GenerateServicesReportTest : BarberBossClassFixture
    {
        private const string METHOD = "api/Report";

        private readonly string _adminToken;
        private readonly string _teamMemberToken;
        private readonly DateTime _fixedDate;

        public GenerateServicesReportTest(CustomWebApplicationFactory webApplicationFactory)
            : base(webApplicationFactory)
        {
            _adminToken = webApplicationFactory.User_Admin.GetToken();
            _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
            _fixedDate = new DateTime(2024, 12, 1);
        }

        [Fact]
        public async Task Success_Pdf()
        {
            var result = await DoGet(
               requestUri: $"{METHOD}/pdf?week={_fixedDate:yyyy-MM-dd}",
               token: _adminToken);

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Content.Headers.ContentType.Should().NotBeNull();
            result.Content.Headers.ContentType!.MediaType.Should()
                  .Be(MediaTypeNames.Application.Pdf);
        }

        [Fact]
        public async Task Error_Forbidden_User_Not_Allowed_Pdf()
        {
            var result = await DoGet(
                requestUri: $"{METHOD}/pdf?month={_fixedDate:yyyy-MM}",
                token: _teamMemberToken);

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
