using System;
using System.Threading;
using System.Threading.Tasks;

namespace 教务管理系统.AiServices
{
    public interface IAiClient : IDisposable
    {
        Task<string> GetResponseAsync(string message, CancellationToken cancellationToken);
        Task<bool> ValidateApiKey(CancellationToken cancellationToken);
        string ApiKey { get; }
        string LastError { get; }
    }
}