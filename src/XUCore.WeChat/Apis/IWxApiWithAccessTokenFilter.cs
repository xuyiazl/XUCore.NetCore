using WebApiClientCore.Attributes;

namespace XUCore.WeChat.Apis
{
    /// <summary>
    /// 
    /// </summary>
    [JsonNetReturn(EnsureMatchAcceptContentType = false)]
    [AccessTokenApiFilter]
    [LoggingFilter]
    public interface IWxApiWithAccessTokenFilter
    {
    }
}
