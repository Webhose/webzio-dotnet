namespace webzio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

#if !NET35 && !NET40
    using System.Threading;
    using System.Threading.Tasks;
#endif

    public class WebzClient
    {
        private readonly WebzOptions options;

        public WebzClient(string token)
        {
            this.options = new WebzOptions {Token = token};
        }

        public WebzClient(WebzOptions options = null)
        {
            this.options = options ?? new WebzOptions();
        }

        public WebzJsonResponseMessage Query(
            string endpoint,
            IDictionary<string, string> parameters)
        {
            var response = Helpers.GetResponseString(GetQueryUri(options, endpoint, parameters));
            return new WebzJsonResponseMessage(response);
        }

#if !NET35 && !NET40
        public async Task<WebzJsonResponseMessage> QueryAsync(
            string endpoint, 
            IDictionary<string, string> parameters)
        {
            var response = await Helpers.GetResponseStringAsync(GetQueryUri(options, endpoint, parameters));
            return new WebzJsonResponseMessage(response);
        }
#endif

        protected static Uri GetQueryUri(
            WebzOptions options,
            string endpoint,
            IDictionary<string, string> parameters)
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
}
