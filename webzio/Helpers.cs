using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace webzio
{
    static class Helpers
    {
        public static async Task<string> GetResponseStringAsync(Uri requestUri)
        {
            var httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);

            EnsureSuccessStatusCode(response.StatusCode);

            return await response.Content.ReadAsStringAsync();
        }

        private static void EnsureSuccessStatusCode(HttpStatusCode statusCode)
        {
            if (statusCode >= (HttpStatusCode)300)
            {
                throw new Exception();
            }
        }
    }
}
