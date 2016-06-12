using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Httwrap.Interface;    

namespace Httwrap
{
    public sealed class HttwrapClient : IHttwrapClient, IDisposable
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

        private readonly HttpClient _httpClient;

        private readonly IQueryStringBuilder _queryStringBuilder;

        public HttwrapClient(IHttwrapConfiguration configuration)
            : this(configuration, new QueryStringBuilder())
        {
        }

        internal HttwrapClient(IHttwrapConfiguration configuration, IQueryStringBuilder queryStringBuilder)
        {
            _configuration = configuration;
            _queryStringBuilder = queryStringBuilder;
            _httpClient = _configuration.GetHttpClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<IHttwrapResponse> GetAsync(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null)
        {
            return await RequestAsync(HttpMethod.Get, path, null, errorHandler, customHeaders);
        }

        public async Task<IHttwrapResponse> GetAsync(string path, object payload,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null)
        {
            path = $"{path}?{_queryStringBuilder.BuildFrom(payload)}";

            return await RequestAsync(HttpMethod.Get, path, null, errorHandler, customHeaders);
        }

        public async Task<IHttwrapResponse<T>> GetAsync<T>(string path,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null)
        {
            return await RequestAsync<T>(HttpMethod.Get, path, null, errorHandler, customHeaders);
        }

        public async Task<IHttwrapResponse<T>> GetAsync<T>(string path, object payload,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null)
        {
            path = $"{path}?{_queryStringBuilder.BuildFrom(payload)}";
            return await RequestAsync<T>(HttpMethod.Get, path, null, errorHandler, customHeaders);
        }

        public async Task<IHttwrapResponse> PutAsync<T>(string path, T data,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null)
        {
            return await RequestAsync(HttpMethod.Put, path, data, errorHandler, customHeaders);
        }

        public async Task<IHttwrapResponse> PostAsync<T>(string path, T data,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null)
        {
            return await RequestAsync(HttpMethod.Post, path, data, errorHandler, customHeaders);
        }

        public async Task<IHttwrapResponse> DeleteAsync(string path, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null)
        {
            return await RequestAsync(HttpMethod.Delete, path, null, errorHandler, customHeaders);
        }

        public async Task<IHttwrapResponse> PatchAsync<T>(string path, T data,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null)
        {
            return await RequestAsync(new HttpMethod("PATCH"), path, data, errorHandler, customHeaders);
        }

        private async Task<IHttwrapResponse> RequestAsync(HttpMethod method, string path, object body,
            Action<HttpStatusCode, string> errorHandler = null, Dictionary<string, string> customHeaders = null)
        {
            var response =
                await
                    RequestAsyncImpl(null, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None, method,
                        path, body, customHeaders);

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            HandleIfErrorResponse(response.StatusCode, content, errorHandler);

            return new HttwrapResponse(response.StatusCode, content);
        }

        private async Task<IHttwrapResponse<T>> RequestAsync<T>(HttpMethod method, string path,
            object body, Action<HttpStatusCode, string> errorHandler = null,
            Dictionary<string, string> customHeaders = null)
        {
            var response =
                await
                    RequestAsyncImpl(null, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None, method,
                        path, body, customHeaders);

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            HandleIfErrorResponse(response.StatusCode, content, errorHandler);

            return new HttwrapResponse<T>(response)
            {
                Data = _configuration.Serializer.Deserialize<T>(content)
            };
        }

        private async Task<HttpResponseMessage> RequestAsyncImpl(TimeSpan? requestTimeout,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, HttpMethod method,
            string path, object body, Dictionary<string, string> customHeaders = null)
        {
            try
            {
                if (requestTimeout.HasValue)
                {
                    _httpClient.Timeout = requestTimeout.Value;
                }

                var request = PrepareRequest(method, body, path, customHeaders);
                return await _httpClient.SendAsync(request, completionOption, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new HttwrapException(
                    $"An error occured while execution request. Path : {path} , HttpMethod : {method}", ex);
            }
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, object body, string path,
            Dictionary<string, string> customHeaders = null)
        {
            var url = $"{_configuration.BasePath}{path}";

            var request = new HttpRequestMessage(method, url);

            request.Headers.Add("User-Agent", UserAgent);

            request.Headers.Add("Accept", "application/json");

            if (customHeaders != null)
                foreach (var header in customHeaders) request.Headers.Add(header.Key, header.Value);

            if (body != null)
            {
                var content = new JsonRequestContent(body, _configuration.Serializer);
                var requestContent = content.GetContent();
                request.Content = requestContent;
            }

            return request;
        }

        private void HandleIfErrorResponse(HttpStatusCode statusCode, string content,
            Action<HttpStatusCode, string> errorHandler)
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