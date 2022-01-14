using Newtonsoft.Json;
using System.Collections.Generic;

namespace XUCore.WeChat.Apis.User.Dtos
{
    public class GetTagsApiResult : ApiResultBase
    {
        [JsonProperty("tags")]
        public IEnumerable<GetTagInfo> Tags { get; set; }
    }

    public class GetTagInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}