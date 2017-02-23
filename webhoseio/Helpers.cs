namespace webhoseio
{
    using System;
    using System.IO;
    using System.Net;

#if !NET35
    using System.Threading.Tasks;
#endif

    static class Helpers
    {
        private static void EnsureSuccessStatusCode(HttpWebResponse response)
        {
            if (response.StatusCode >= (HttpStatusCode)300)
            {
                throw new Exception();
            }
        }

        public static string GetResponseString(Uri requestUri)
        {
            var request = (HttpWebRequest) WebRequest.Create(requestUri);
            var response = (HttpWebResponse) request.GetResponse();
            EnsureSuccessStatusCode(response);
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

#if !NET35 && !NET40
        public static async Task<string> GetResponseStringAsync(Uri requestUri)
        {
            var request = (HttpWebRequest) WebRequest.Create(requestUri);
            var response = (HttpWebResponse) await request.GetResponseAsync();
            EnsureSuccessStatusCode(response);
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return await sr.ReadToEndAsync();
            }
        }
#endif
    }
}