using Newtonsoft.Json;

namespace XUCore.WeChat.Apis.User.Dtos
{
    public class GetIdListInput
    {
        [JsonProperty("openid")]
        public string OpenId { get; set; }
    }
}