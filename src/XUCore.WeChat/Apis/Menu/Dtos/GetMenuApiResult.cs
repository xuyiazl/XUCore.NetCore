using Newtonsoft.Json;

namespace XUCore.WeChat.Apis.Menu
{
    public class GetMenuApiResult : ApiResultBase
    {
        [JsonProperty("menu")]
        public MenuInfo Menu { get; set; }
    }
}