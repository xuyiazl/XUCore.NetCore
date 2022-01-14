using Newtonsoft.Json;

namespace XUCore.WeChat.Apis.CustomerService
{
    public class DelKfAccountInput
    {
        [JsonProperty("kf_account")]
        public string Account { get; set; }
    }
}