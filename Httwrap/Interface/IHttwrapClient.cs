using System;
using System.Net;
using System.Threading.Tasks;

namespace Httwrap.Interface
{
    public interface IHttwrapClient
    {
        Task<IHttwrapResponse> GetAsync(string path, Action<HttpStatusCode, string> errorHandler = null);
        Task<IHttwrapResponse<T>> GetAsync<T>(string path, Action<HttpStatusCode, string> errorHandler = null);
        Task<IHttwrapResponse> PutAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null);
        Task<IHttwrapResponse> PostAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null);
        Task<IHttwrapResponse> DeleteAsync(string path, Action<HttpStatusCode, string> errorHandler = null);
        Task<IHttwrapResponse> PatchAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null);
    }
}