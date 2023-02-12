using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThorQ_Rapid
{
    internal static class HttpUtils
    {
        public static async Task<string> HttpGetStringAsync(string url, Dictionary<string, string> headers, HttpClient httpClient)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(url)))
            {
                if (headers != null)
                {
                    foreach (var entry in headers)
                        request.Headers.Add(entry.Key, entry.Value);
                }

                using (HttpResponseMessage response = await httpClient.SendAsync(request))
                {
                    return await response.Content.ReadAsStringAsync();
                }

            }
        }
        public static async Task<string> HttpGetStringAsync(string url, Dictionary<string, string> headers)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                return await HttpGetStringAsync(url, headers, httpClient);
            }
        }
        public static async Task<string> HttpGetStringAsync(string url, HttpClient httpClient)
        {
            return await HttpGetStringAsync(url, new Dictionary<string, string>(), httpClient);
        }
        public static async Task<string> HttpGetStringAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                return await HttpGetStringAsync(url, new Dictionary<string, string>(), httpClient);
            }
        }
    }

}