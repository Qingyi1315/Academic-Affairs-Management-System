using System.Threading;
using System.Threading.Tasks;

namespace 教务管理系统.AiServices
{
    public class Kimi : AiClientBase
    {
        protected override string ModelName => "moonshot-v1-8k";
        protected override string ApiUrl => "https://api.moonshot.cn/v1/chat/completions";

        public Kimi(string apiKey) : base(apiKey) { }

        public override async Task<bool> ValidateApiKey(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _webService.GetAsync(
                    "https://api.moonshot.cn/v1/models",
                    "Bearer",
                    ApiKey,
                    cancellationToken
                );
                return !string.IsNullOrEmpty(response) && response.Contains("data");
            }
            catch
            {
                return false;
            }
        }
    }
}