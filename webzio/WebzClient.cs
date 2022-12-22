namespace webzio;

public class WebzClient
{
    private readonly WebzOptions _options;

    public WebzClient(string token)
    {
        _options = new WebzOptions
        {
            Token = token
        };
    }

    public async Task<WebzJsonResponseMessage> QueryAsync(string endpoint, IDictionary<string, string> parameters)
    {
        var response = await Helpers.GetResponseStringAsync(GetQueryUri(_options, endpoint, parameters));
        return new WebzJsonResponseMessage(response);
    }

    protected static Uri GetQueryUri(WebzOptions options, string endpoint, IDictionary<string, string> parameters)
    {
        if (parameters == null)
        {
            parameters = new Dictionary<string, string>();
        }
        if (!parameters.ContainsKey("token"))
        {
            parameters.Add("token", options.Token);
        }
        if (!parameters.ContainsKey("format"))
        {
            parameters.Add("format", options.Format);
        }

        var query = string.Join("&", parameters.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}").ToArray());
        query = string.IsNullOrEmpty(query) ? query : "?" + query;

        return new Uri(Constants.BaseUri + endpoint + query);
    }
}
