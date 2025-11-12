using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace PlaywrightTestsXUnit
{

    public class UnitTest1 : IClassFixture<PlaywrightFixture>
    {
        private readonly PlaywrightFixture _fixture;

        public UnitTest1(PlaywrightFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Test_HomePage_Title()
        {
            var response = await _fixture.Page.GotoAsync($"{_fixture.BaseUrl}/block");
            Assert.True(response?.Ok ?? false);
        }
    }
}