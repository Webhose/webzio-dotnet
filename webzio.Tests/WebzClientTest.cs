using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace webzio.Tests;

public class WebzClientTest
{
    private readonly ITestOutputHelper _console;

    public WebzClientTest(ITestOutputHelper console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    [Fact]
    public async Task SimpleTest()
    {
        string token = "3ff39d5a-5c2d-41ee-b785-36b0cc520170";

        var client = new WebzClient(token);
        var output = await client.QueryAsync("search", new Dictionary<string, string> { { "q", "github" } });

        _console.WriteLine(output["posts"][0]["text"].ToString());
        _console.WriteLine(output["posts"][0]["published"].ToString());

        _console.WriteLine(output["posts"].Count().ToString());
        _console.WriteLine(output["posts"][0]["language"].Count().ToString());

        output = await output.GetNextAsync();
        _console.WriteLine(output["posts"][0]["thread"]["site"].ToString());
    }
}