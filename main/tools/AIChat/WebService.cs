using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 教务管理系统.AiServices
{
    public class WebService : IDisposable
    {
        private readonly HttpClient _httpClient;
        public string LastError { get; private set; }
        private bool _disposed = false;

        public WebService(int timeoutSeconds = 30)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public async Task<string> PostAsync(
            string url,
            string jsonData,
            string authPrefix,
            string apiKey,
            CancellationToken cancellationToken)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url)) // 修复1：传统using块
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(authPrefix, apiKey);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    var response = await _httpClient.SendAsync(request, cancellationToken);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        HandleErrorStatus(response.StatusCode);
                        return null;
                    }

                    return responseContent;
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return null;
            }
        }

        public async Task<string> GetAsync(
            string url,
            string authPrefix,
            string apiKey,
            CancellationToken cancellationToken)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url)) // 修复2：传统using块
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(authPrefix, apiKey);

                    var response = await _httpClient.SendAsync(request, cancellationToken);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return null;
            }
        }

        private void HandleErrorStatus(HttpStatusCode statusCode)
        {
            // 修复3：传统switch语句
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    LastError = "认证失败";
                    break;
                case (HttpStatusCode)429: // 修复4：使用数值代替TooManyRequests
                    LastError = "请求过于频繁";
                    break;
                default:
                    LastError = $"HTTP错误: {(int)statusCode}";
                    break;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _httpClient?.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}