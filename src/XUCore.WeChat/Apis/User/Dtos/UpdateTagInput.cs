using Newtonsoft.Json;

namespace XUCore.WeChat.Apis.User.Dtos
{
    public class UpdateTagInput
    {
        [JsonProperty("tag")]
        public UpdateTagInfo Tag { get; set; }
    }

    public class UpdateTagInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}