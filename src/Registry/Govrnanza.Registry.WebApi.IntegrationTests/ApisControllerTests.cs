using Govrnanza.Registry.WebApi.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Govrnanza.Registry.WebApi.IntegrationTests
{
    public class ApisControllerTests
    {
        private TestFixture _testFixture;

        public ApisControllerTests()
        {
            _testFixture = new TestFixture();
        }
        
        /// <summary>
        /// This is a bad test, it is just to verify requests can be made via the TestServer
        /// </summary>
        [Fact]
        public async Task WhenGet_ThenReturnsOk()
        {
            var response = await _testFixture.Client.GetAsync("/api/v1/apis");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }
    }
}
