using Newtonsoft.Json;
using System.Collections.Generic;

namespace XUCore.WeChat.Apis.Menu
{
    public class CreateMenuInput
    {
        [JsonProperty("button")]
        public List<MenuButtonBase> Button { get; set; }
    }
}