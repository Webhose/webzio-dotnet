namespace webzio
{
    using System;
    using Newtonsoft.Json.Linq;

#if !NET35 && !NET40
    using System.Threading;
    using System.Threading.Tasks;
#endif

    public class WebzJsonResponseMessage
    {
        public JObject Json { get; }

        public JToken this[string key] => Json[key];

        internal WebzJsonResponseMessage(string content)
        {
            Json = JObject.Parse(content);
        }

        public WebzJsonResponseMessage GetNext()
        {
            var response = Helpers.GetResponseString(GetNextUri(Json));
            return new WebzJsonResponseMessage(response);
        }

#if !NET35 && !NET40
        public async Task<WebzJsonResponseMessage> GetNextAsync()
        {
            var response = await Helpers.GetResponseStringAsync(GetNextUri(Json));
            return new WebzJsonResponseMessage(response);
        }
#endif

        protected static Uri GetNextUri(JObject json)
        {
            return new Uri(Constants.BaseUri + json["next"].Value<string>());
        }
    }
}