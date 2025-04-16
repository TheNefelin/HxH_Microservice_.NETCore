using Grpc.Core;

namespace TestProject.UnitTests.gRPC.Tests;

public class TestServerCallContext : ServerCallContext
{
    protected override string MethodCore => "TestMethod";
    protected override string HostCore => "localhost";
    protected override string PeerCore => "localhost";
    protected override DateTime DeadlineCore => DateTime.UtcNow.AddMinutes(1);
    protected override Metadata RequestHeadersCore => new Metadata();
    protected override CancellationToken CancellationTokenCore => CancellationToken.None;
    protected override Metadata ResponseTrailersCore => new Metadata();
    protected override Status StatusCore { get; set; }
    protected override WriteOptions WriteOptionsCore { get; set; }
    protected override AuthContext AuthContextCore => new AuthContext("test", new Dictionary<string, List<AuthProperty>>());

    protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions options) 
        => null;

    protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        => Task.CompletedTask;

    public static ServerCallContext Create() => new TestServerCallContext();
}