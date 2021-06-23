namespace XUCore.NetCore.Oss
{
    public interface IOssMultiPartFactory
    {
        bool CreateClient(string name, OssOptions options);
        bool RemoveClient(string token);
        IOssMultiPartClient GetClient(string name, string token, string relativePath);
    }
}