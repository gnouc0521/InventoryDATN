using System.Threading.Tasks;
using bbk.netcore.Models.TokenAuth;
using bbk.netcore.Web.Controllers;
using Shouldly;
using Xunit;

namespace bbk.netcore.Web.Tests.Controllers
{
    public class HomeController_Tests: netcoreWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}