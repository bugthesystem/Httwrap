using System;
using System.Net;
using System.Threading.Tasks;

namespace Httwrap.Interface
{
    public interface IHttwrapClient
    {
        Task<HttwrapResponse> GetAsync(string path, Action<HttpStatusCode, string> errorHandler = null);
        Task<HttwrapResponse> PutAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null);
        Task<HttwrapResponse> PostAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null);
        Task<HttwrapResponse> DeleteAsync(string path, Action<HttpStatusCode, string> errorHandler = null);
        Task<HttwrapResponse> PatchAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null);
    }
}