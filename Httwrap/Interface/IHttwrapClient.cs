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
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);
        
        IHttwrapResponse Get(string path, object payload, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        IHttwrapResponse Get(string path, Action<HttpStatusCode, string> errorHandler = null,
           Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        Task<IHttwrapResponse> GetAsync(string path, object payload, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        Task<IHttwrapResponse<T>> GetAsync<T>(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        Task<IHttwrapResponse<T>> GetAsync<T>(string path, object payload,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        Task<IHttwrapResponse> PutAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        IHttwrapResponse Put<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        Task<IHttwrapResponse> PostAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        IHttwrapResponse Post<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        Task<IHttwrapResponse> DeleteAsync(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        IHttwrapResponse Delete(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        Task<IHttwrapResponse> PatchAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        IHttwrapResponse Patch<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null, TimeSpan? requestTimeout = null);

        void AddInterceptor(IHttpInterceptor interceptor);
    }
}