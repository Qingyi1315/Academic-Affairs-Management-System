using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace 教务管理系统.AiServices
{
    public abstract class AiClientBase : IAiClient
    {
        protected readonly WebService _webService;
        public string ApiKey { get; }
        public string LastError => _webService.LastError;
        protected abstract string ModelName { get; }
        protected abstract string ApiUrl { get; }

        protected AiClientBase(string apiKey)
        {
            _webService = new WebService();
            ApiKey = apiKey;
        }

        public virtual async Task<string> GetResponseAsync(string message, CancellationToken cancellationToken)
        {
            var request = new
            {
                model = ModelName,
                messages = new[] { new { role = "user", content = message } }
            };

            var response = await _webService.PostAsync(
                ApiUrl,
                JsonConvert.SerializeObject(request),
                "Bearer",
                ApiKey,
                cancellationToken
            );

            return ParseResponse(response);
        }

        protected virtual string ParseResponse(string response)
        {
            try
            {
                dynamic json = JsonConvert.DeserializeObject(response);
                return json.choices[0].message.content;
            }
            catch
            {
                throw new Exception("响应解析失败");
            }
        }

        public virtual async Task<bool> ValidateApiKey(CancellationToken cancellationToken)
        {
            try
            {
                var testRequest = new
                {
                    model = ModelName,
                    messages = new[] { new { role = "user", content = "ping" } },
                    max_tokens = 1
                };

                var response = await _webService.PostAsync(
                    ApiUrl,
                    JsonConvert.SerializeObject(testRequest),
                    "Bearer",
                    ApiKey,
                    cancellationToken
                );

                return !string.IsNullOrEmpty(response) && response.Contains("id");
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _webService?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}