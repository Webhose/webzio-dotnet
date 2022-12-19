namespace webzio.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using webzio;
    using Xunit;
    using Xunit.Abstractions;

#if !NET35 && !NET40
    using System.Threading;
    using System.Threading.Tasks;
#endif

    public class WebzClientTest
    {
        private readonly ITestOutputHelper console;

        public WebzClientTest(ITestOutputHelper console)
        {
            this.console = console;
#if !NET35 && !NET40
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
        }

#if !NET35 && !NET40
        [Fact]
        public async Task SimpleTest()
        {
            var client = new WebzClient();
            var output = await client.QueryAsync("search", new Dictionary<string, string> { { "q", "github" } });

            console.WriteLine(output["posts"][0]["text"].ToString());
            console.WriteLine(output["posts"][0]["published"].ToString());

            console.WriteLine(output["posts"].Count().ToString());
            console.WriteLine(output["posts"][0]["language"].Count().ToString());

            output = await output.GetNextAsync();
            console.WriteLine(output["posts"][0]["thread"]["site"].ToString());
        }
#endif
    }
}
