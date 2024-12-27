using DarkSky.Core.Services;
using DarkSky.Core.Tests.Fixtures;
using DarkSky.Core.Tests.Stubs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSky.Core.Tests.UnitTests
{
	public class ATProtoServiceTest : IClassFixture<ATProtoFixture>
	{
		private ATProtoFixture fixture;

		public ATProtoServiceTest(ATProtoFixture fixture) => this.fixture = fixture;

		[Fact]
		public async Task LoginTest()
		{
			StubMessengerListener listener = new StubMessengerListener();
			fixture.proto = new ATProtoService(new StubKeyService());
			await fixture.proto.LoginAsync(fixture.Username, fixture.Password);

			Assert.NotNull(fixture.proto.ATProtocolClient.Session);
			Assert.Equal("firecubestudios.bsky.social", fixture.proto.ATProtocolClient.Session.Handle.Handle);
			Assert.NotNull(listener.session);
		}

		[Fact]
		public async Task RefreshLoginTest()
		{
			StubMessengerListener listener = new StubMessengerListener();
			var proto = fixture.proto.ATProtocolClient;
			fixture.proto = new ATProtoService(new StubKeyService());
			await fixture.proto.RefreshLoginAsync(proto.Session.Did.ToString(), proto.Session.Handle.Handle, proto.Session.AccessJwt, proto.Session.RefreshJwt);

			Assert.NotNull(fixture.proto.ATProtocolClient.Session);
			Assert.Equal("firecubestudios.bsky.social", fixture.proto.ATProtocolClient.Session.Handle.Handle);
			Assert.NotNull(listener.session);
		}
	}
}
