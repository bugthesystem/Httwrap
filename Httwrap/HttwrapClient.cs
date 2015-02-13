using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Httwrap.Interface;

namespace Httwrap
{
    public sealed class HttwrapClient : IHttwrapClient
    {
        private const string UserAgent = "Httwrap";
        private readonly IHttwrapConfiguration _configuration;

        private readonly Action<HttpStatusCode, string> _defaultErrorHandler = (statusCode, body) =>
        {
            if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
            {
                throw new HttwrapHttpException(statusCode, body);
            }
        };

        public HttwrapClient(IHttwrapConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IHttwrapResponse> GetAsync(string path, Action<HttpStatusCode, string> errorHandler = null)
        {
            return await RequestAsync(HttpMethod.Get, path, null, errorHandler);
        }

        public async Task<IHttwrapResponse<T>> GetAsync<T>(string path, Action<HttpStatusCode, string> errorHandler = null)
        {
            return await RequestAsync<T>(HttpMethod.Get, path, null, errorHandler);
        }

        public async Task<IHttwrapResponse> PutAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null)
        {
            return await RequestAsync(new HttpMethod("PATCH"), path, data, errorHandler);
        }

        public async Task<IHttwrapResponse> PostAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null)
        {
            return await RequestAsync(HttpMethod.Post, path, data, errorHandler);
        }

        public async Task<IHttwrapResponse> DeleteAsync(string path, Action<HttpStatusCode, string> errorHandler = null)
        {
            return await RequestAsync(HttpMethod.Delete, path, null, errorHandler);
        }

        public async Task<IHttwrapResponse> PatchAsync<T>(string path, T data, Action<HttpStatusCode, string> errorHandler = null)
        {
            return await RequestAsync(new HttpMethod("PATCH"), path, data, errorHandler);
        }

        private async Task<IHttwrapResponse> RequestAsync(HttpMethod method, string path, object body,
            Action<HttpStatusCode, string> errorHandler = null)
        {
            var response =
                await
                    RequestInnerAsync(null, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None, method,
                        path, body);

            var content = await response.Content.ReadAsStringAsync();

            HandleIfErrorResponse(response.StatusCode, content, errorHandler);

            return new HttwrapResponse(response.StatusCode, content);
        }

        private async Task<IHttwrapResponse<T>> RequestAsync<T>(HttpMethod method, string path, object body,
            Action<HttpStatusCode, string> errorHandler = null)
        {
            var response =
                await
                    RequestInnerAsync(null, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None, method,
                        path, body);

            var content = await response.Content.ReadAsStringAsync();

            HandleIfErrorResponse(response.StatusCode, content, errorHandler);

            return new HttwrapResponse<T>(response)
            {
                Data = _configuration.Serializer.Deserialize<T>(content)
            };
        }

        private async Task<HttpResponseMessage> RequestInnerAsync(TimeSpan? requestTimeout,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, HttpMethod method, string path,
            object body)
        {
            try
            {
                var client = _configuration.GetHttpClient();

                if (requestTimeout.HasValue)
                {
                    client.Timeout = requestTimeout.Value;
                }

                var request = PrepareRequest(method, body, path);

                return await client.SendAsync(request, completionOption, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new HttwrapException(
                    string.Format("An error occured while execution request. Path : {0} , HttpMethod : {1}", path,
                        method), ex);
            }
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, object body, string path)
        {
            var url = string.Format("{0}{1}", _configuration.BasePath, path);

            var request = new HttpRequestMessage(method, url);

            request.Headers.Add("User-Agent", UserAgent);

            request.Headers.Add("Accept", "application/json");

            if (body != null)
            {
                var content = new JsonRequestContent(body, _configuration.Serializer);
                var requestContent = content.GetContent();
                request.Content = requestContent;
            }

            return request;
        }

        private void HandleIfErrorResponse(HttpStatusCode statusCode, string content, Action<HttpStatusCode, string> errorHandler)
        {
            if (errorHandler != null)
            {
                errorHandler(statusCode, content);
            }
            else
            {
                _defaultErrorHandler(statusCode, content);
            }
        }
    }
}