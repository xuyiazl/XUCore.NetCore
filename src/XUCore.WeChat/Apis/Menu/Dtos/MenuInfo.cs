using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace XUCore.WeChat.Apis.Menu
{
    /// <summary>
    ///     菜单信息
    /// </summary>
    public class MenuInfo
    {
        [JsonProperty("button")]
        public List<MenuButtonBase> Button { get; set; }
    }
}