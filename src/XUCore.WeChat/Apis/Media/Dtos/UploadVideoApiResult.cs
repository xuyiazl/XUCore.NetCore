namespace XUCore.WeChat.Apis.Media
{
    public class UploadVideoApiResult : ApiResultBase
    {
        public string type { get; set; }
        public string media_id { get; set; }
        public int created_at { get; set; }
    }
}