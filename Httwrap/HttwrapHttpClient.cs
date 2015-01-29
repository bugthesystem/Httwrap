using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Httwrap.Interface;

namespace Httwrap
{
    public delegate void HttwrapErrorHandlingDelegate(HttpStatusCode statusCode, string responseBody);

    public sealed class HttwrapHttpClient : IHttwrapHttpClient
    {
        private const string UserAgent = "Httwrap";
        private readonly IHttwrapConfiguration _configuration;

        private readonly HttwrapErrorHandlingDelegate _defaultErrorHandlingDelegate = (statusCode, body) =>
        {
            if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
            {
                throw new HttwrapHttpException(statusCode, body);
            }
        };

        public HttwrapHttpClient(IHttwrapConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HttwrapResponse> GetAsync(string path, HttwrapErrorHandlingDelegate errorHandler = null)
        {
            return await RequestAsync(HttpMethod.Get, path, null, errorHandler);
        }

        public async Task<HttwrapResponse> PutAsync<T>(string path, T data,
            HttwrapErrorHandlingDelegate errorHandler = null)
        {
            return await RequestAsync(new HttpMethod("PATCH"), path, data, errorHandler);
        }

        public async Task<HttwrapResponse> PostAsync<T>(string path, T data,
            HttwrapErrorHandlingDelegate errorHandler = null)
        {
            return await RequestAsync(HttpMethod.Post, path, data, errorHandler);
        }

        public async Task<HttwrapResponse> DeleteAsync(string path, HttwrapErrorHandlingDelegate errorHandler = null)
        {
            return await RequestAsync(HttpMethod.Delete, path, null, errorHandler);
        }

        public async Task<HttwrapResponse> PatchAsync<T>(string path, T data,
            HttwrapErrorHandlingDelegate errorHandler = null)
        {
            return await RequestAsync(new HttpMethod("PATCH"), path, data, errorHandler);
        }

        private HttpClient GetHttpClient()
        {
            return new HttpClient();
        }

        private async Task<HttwrapResponse> RequestAsync(HttpMethod method, string path, object body,
            HttwrapErrorHandlingDelegate errorHandler = null)
        {
            var response =
                await
                    RequestInnerAsync(null, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None, method,
                        path, body);

            var content = await response.Content.ReadAsStringAsync();

            HandleIfErrorResponse(response.StatusCode, content, errorHandler ?? ((statusCode, responseBody) => { }));

            return new HttwrapResponse(response.StatusCode, content);
        }

        private async Task<HttpResponseMessage> RequestInnerAsync(TimeSpan? requestTimeout,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, HttpMethod method, string path,
            object body)
        {
            try
            {
                var client = GetHttpClient();

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
                var content = new JsonRequestContent(body, new JsonSerializerWrapper());
                var requestContent = content.GetContent();
                request.Content = requestContent;
            }

            return request;
        }

        private void HandleIfErrorResponse(HttpStatusCode statusCode, string responseBody,
            HttwrapErrorHandlingDelegate errorHandler)
        {
            if (errorHandler == null)
            {
                throw new ArgumentNullException("errorHandler");
            }

            errorHandler(statusCode, responseBody);
            _defaultErrorHandlingDelegate(statusCode, responseBody);
        }
    }
}