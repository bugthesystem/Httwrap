using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Httwrap.Interface
{
    public interface IHttwrapClient
    {
        Task<IHttwrapResponse> GetAsync(string path, Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);
        Task<IHttwrapResponse> GetAsync(string path, object payload, Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);
        Task<IHttwrapResponse<T>> GetAsync<T>(string path, Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);

        Task<IHttwrapResponse<T>> GetAsync<T>(string path, object payload,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);

        Task<IHttwrapResponse> PutAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);
        Task<IHttwrapResponse> PostAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);
        Task<IHttwrapResponse> DeleteAsync(string path, Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);
        Task<IHttwrapResponse> PatchAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> extraHeaders = null);
    }
}