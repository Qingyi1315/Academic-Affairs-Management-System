namespace 教务管理系统.AiServices
{
    public class DeepSeek : AiClientBase
    {
        protected override string ModelName => "deepseek-chat";
        protected override string ApiUrl => "https://api.deepseek.com/v1/chat/completions";

        public DeepSeek(string apiKey) : base(apiKey) { }
    }
}