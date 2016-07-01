using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Httwrap.Interception;

namespace Httwrap.Interface
{
    public interface IHttwrapClient
    {
        Task<IHttwrapResponse> GetAsync(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);
        
        IHttwrapResponse Get(string path, object payload, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        Task<IHttwrapResponse> GetAsync(string path, object payload, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        Task<IHttwrapResponse<T>> GetAsync<T>(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        Task<IHttwrapResponse<T>> GetAsync<T>(string path, object payload,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null);

        Task<IHttwrapResponse> PutAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        IHttwrapResponse Put<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        Task<IHttwrapResponse> PostAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        IHttwrapResponse Post<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        Task<IHttwrapResponse> DeleteAsync(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        IHttwrapResponse Delete(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        Task<IHttwrapResponse> PatchAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        IHttwrapResponse Patch<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null);

        void AddInterceptor(IHttpInterceptor interceptor);
    }
}