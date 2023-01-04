using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace webzio
{
    public class WebzJsonResponseMessage
    {
        public JObject Json { get; }

        public JToken this[string key] => Json[key];

        internal WebzJsonResponseMessage(string content)
        {
            Json = JObject.Parse(content);
        }

        public async Task<WebzJsonResponseMessage> GetNextAsync()
        {
            var response = await Helpers.GetResponseStringAsync(GetNextUri(Json));
            return new WebzJsonResponseMessage(response);
        }

        protected static Uri GetNextUri(JObject json)
        {
            return new Uri(Constants.BaseUri + json["next"].Value<string>());
        }
    }
}
