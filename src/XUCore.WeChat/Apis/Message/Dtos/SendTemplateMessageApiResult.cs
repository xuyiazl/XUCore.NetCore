using Newtonsoft.Json;

namespace XUCore.WeChat.Apis.Message
{
    public class SendTemplateMessageApiResult : ApiResultBase
    {
        [JsonProperty("msgid")]
        public string MessageId { get; set; }
    }
}