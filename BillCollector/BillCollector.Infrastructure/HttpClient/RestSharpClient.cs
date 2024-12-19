using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BillCollector.Infrastructure.HttpClient
{
    public class RestSharpClient : IRestSharpClient
    {
        private RestClient CreateClient(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromMinutes(5)
            };

            var client = new RestClient(baseUrl);
            return client;
        }

        private RestRequest CreateRequest(string resource, Method method, Dictionary<string, string> headers = null)
        {
            var request = new RestRequest(resource, method);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }
            return request;
        }

        private string ExcludeNullValues(object body, bool excludeNull = true)
        {
            string json = string.Empty;
            if (body != null)
            {
                try
                {
                    if (excludeNull)
                    {
                        json = JsonConvert.SerializeObject(body, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    }
                    else
                    {
                        json = JsonConvert.SerializeObject(body);
                    }
                }
                catch(Exception ex)
                {

                }
            }
            return json;
        }
        
        private string ApplyRemitaFormatting(string baseUrl, string resource, object body)
        {
            string json = string.Empty;
            if (body != null)
            {
                try
                {
                    if (baseUrl.Contains("remita") || resource.Contains("remita"))
                    {
                        json = JsonConvert.SerializeObject(body, new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            Formatting = Formatting.Indented
                        }
                        );
                    }
                }
                catch(Exception ex)
                {

                }
            }
            return json;
        }

        public async Task<RestResponse> GetAsync(string baseUrl, string resource, Dictionary<string, string> headers = null)
        {
            var client = CreateClient(baseUrl);
            var request = CreateRequest(resource, Method.Get, headers);
            return await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> PostAsync(string baseUrl, string resource, object body, Dictionary<string, string> headers = null, bool excludeNull = true)
        {
            body =  ExcludeNullValues(body, excludeNull);
            if (baseUrl.Contains("remita") || resource.Contains("remita"))
            {
                body = ApplyRemitaFormatting(baseUrl, resource, body);
            }
            var client = CreateClient(baseUrl);
            var request = CreateRequest(resource, Method.Post, headers);
            request.AddJsonBody(body);
            return await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> PutAsync(string baseUrl, string resource, object body, Dictionary<string, string> headers = null, bool excludeNull = true)
        {
            body = ExcludeNullValues(body, excludeNull);
            if (baseUrl.Contains("remita") || resource.Contains("remita"))
            {
                body = ApplyRemitaFormatting(baseUrl, resource, body);
            }
            var client = CreateClient(baseUrl);
            var request = CreateRequest(resource, Method.Put, headers);
            request.AddJsonBody(body);
            return await client.ExecuteAsync(request);
        }

        public async Task<RestResponse> DeleteAsync(string baseUrl, string resource, Dictionary<string, string> headers = null)
        {
            var client = CreateClient(baseUrl);
            var request = CreateRequest(resource, Method.Delete, headers);
            return await client.ExecuteAsync(request);
        }
    }

    public class RequestHeaders
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
