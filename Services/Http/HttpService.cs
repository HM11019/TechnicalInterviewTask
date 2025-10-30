using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Text;

namespace WebApplicationNewJob.Services.Http
{
    public class HttpService : IHttpService
    {

        private readonly HttpClient _httpClient;
        private readonly string logPath;
        private readonly IWebHostEnvironment _env;

        public HttpService(HttpClient httpClient, IWebHostEnvironment env)
        {
            _env = env;
            logPath = _env.ContentRootPath;

            _httpClient = httpClient;
            HttpClientHandler _httpHandler = new HttpClientHandler();
            _httpHandler.Proxy = null;
            _httpHandler.UseProxy = false;
            _httpClient = new HttpClient(_httpHandler);
            _httpClient.Timeout = TimeSpan.FromMinutes(6);
        }

        public async Task<string> PostAsync(object obj, string url)
        {
            string result = string.Empty;

            try
            {
                var jsonContent = JsonConvert.SerializeObject(obj); // Serialize the obj to a json string.
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using (var response = await _httpClient.PostAsync(url, content))
                {
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();
                }

            }
            catch (HttpRequestException ex)
            {
                // Handle the http request error                
                throw;
            }
            catch (Exception ex)
            {
                // Handle the any other exception       
                throw;
            }

            return result;
        }

        public async Task<string> GetAsync(string url)
        {
            string result = string.Empty;

            try
            {
                using (var response = await _httpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();
                }

            }
            catch (HttpRequestException ex)
            {
                // Handle the http request error                
                throw;
            }
            catch (Exception ex)
            {
                // Handle the any other exception       
                throw;
            }

            return result;
        }

        public async Task<List<T>> ListFromApiAsync<T>(T model, string endpoint)
        {
            List<T> modelList = new List<T>();

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (HttpClient client = new HttpClient(handler))
            {
                using (var Response = await client.GetAsync(endpoint))
                {
                    // If Status OK, fill list with result
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await Response.Content.ReadAsStringAsync();
                        modelList = JsonConvert.DeserializeObject<List<T>>(apiResponse);
                    }
                }
            }

            return modelList;
        }
    }
}
