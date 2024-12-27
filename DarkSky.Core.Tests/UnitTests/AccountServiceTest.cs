using DarkSky.Core.Services;
using DarkSky.Core.Tests.Fixtures;
using Microsoft.Extensions.Configuration;

namespace DarkSky.Core.Tests.UnitTests
{
	public class AccountServiceTest : IClassFixture<ATProtoFixture>
	{
		private ATProtoFixture fixture;

		public AccountServiceTest(ATProtoFixture fixture) => this.fixture = fixture;

		[Fact]
		public async Task GetProfileTest()
		{
			AccountService accountService = new(fixture.proto);
			var profile = await accountService.GetCurrentProfileAsync();

			Assert.NotNull(profile.Handle);
			Assert.Equal("firecubestudios.bsky.social", profile.Handle!.Handle);
		}
	}
}
