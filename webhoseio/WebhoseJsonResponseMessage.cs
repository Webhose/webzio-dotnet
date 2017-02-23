namespace webhoseio
{
    using System;
    using Newtonsoft.Json.Linq;

#if !NET35 && !NET40
    using System.Threading;
    using System.Threading.Tasks;
#endif

    public class WebhoseJsonResponseMessage
    {
        public JObject Json { get; }

        public JToken this[string key] => Json[key];

        internal WebhoseJsonResponseMessage(string content)
        {
            Json = JObject.Parse(content);
        }

        public WebhoseJsonResponseMessage GetNext()
        {
            var response = Helpers.GetResponseString(GetNextUri(Json));
            return new WebhoseJsonResponseMessage(response);
        }

#if !NET35 && !NET40
        public async Task<WebhoseJsonResponseMessage> GetNextAsync()
        {
            var response = await Helpers.GetResponseStringAsync(GetNextUri(Json));
            return new WebhoseJsonResponseMessage(response);
        }
#endif

        protected static Uri GetNextUri(JObject json)
        {
            return new Uri(Constants.BaseUri + json["next"].Value<string>());
        }
    }
}