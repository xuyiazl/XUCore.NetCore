using System.Threading.Tasks;

namespace XUCore.WeChat
{
    public interface ITokenManager
    {
        Task<string> GetAccessTokenAsync();
    }

    public interface ITicketManager
    {
        Task<string> GetTicketAsync();
    }

    public interface IWebShareSignature
    {
        /// <summary>
        /// 获取网页授权签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<WebShareSignatureResult> Create(string url);
    }
}