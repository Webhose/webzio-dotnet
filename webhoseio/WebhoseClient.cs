namespace webhoseio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

#if !NET35 && !NET40
    using System.Threading;
    using System.Threading.Tasks;
#endif

    public class WebhoseClient
    {
        private readonly WebhoseOptions options;

        public WebhoseClient(string token)
        {
            this.options = new WebhoseOptions {Token = token};
        }

        public WebhoseClient(WebhoseOptions options = null)
        {
            this.options = options ?? new WebhoseOptions();
        }

        public WebhoseJsonResponseMessage Query(
            string endpoint,
            IDictionary<string, string> parameters)
        {
            var response = Helpers.GetResponseString(GetQueryUri(options, endpoint, parameters));
            return new WebhoseJsonResponseMessage(response);
        }

#if !NET35 && !NET40
        public async Task<WebhoseJsonResponseMessage> QueryAsync(
            string endpoint, 
            IDictionary<string, string> parameters)
        {
            var response = await Helpers.GetResponseStringAsync(GetQueryUri(options, endpoint, parameters));
            return new WebhoseJsonResponseMessage(response);
        }
#endif

        protected static Uri GetQueryUri(
            WebhoseOptions options,
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
