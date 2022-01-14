using Newtonsoft.Json;

namespace XUCore.WeChat.Apis.Message
{
    public class GetTemplateIdApiResult : ApiResultBase
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }


    }
}