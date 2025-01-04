using DarkSky.Core.Services;
using DarkSky.Core.Tests.Stubs;
using DarkSky.Core.Tests.UnitTests;
using Microsoft.Extensions.Configuration;

namespace DarkSky.Core.Tests.Fixtures
{
    public class ATProtoFixture : IAsyncLifetime
    {
        internal ATProtoService proto { get; set; } = new ATProtoService(new StubKeyService());
        internal string Username { get; private set; }
        internal string Password { get; private set; }

        public async Task InitializeAsync()
        {
            var secrets = new ConfigurationBuilder()
                .AddUserSecrets<AccountServiceTest>()
                .Build();

            Username = secrets["USERNAME"] ?? throw new InvalidOperationException("USERNAME is missing");
            Password = secrets["PASSWORD"] ?? throw new InvalidOperationException("PASSWORD is missing");

            await proto.LoginAsync(secrets["USERNAME"]!, secrets["PASSWORD"]!);
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
