using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Infrastructure.HttpClient
{
    public interface IRestSharpClient
    {
        Task<RestResponse> GetAsync(string baseUrl, string resource, Dictionary<string, string> headers = null);
        Task<RestResponse> PostAsync(string baseUrl, string resource, object body, Dictionary<string, string> headers = null, bool excludeNull = true);
        Task<RestResponse> PutAsync(string baseUrl, string resource, object body, Dictionary<string, string> headers = null, bool excludeNull = true);
        Task<RestResponse> DeleteAsync(string baseUrl, string resource, Dictionary<string, string> headers = null);
    }
}
