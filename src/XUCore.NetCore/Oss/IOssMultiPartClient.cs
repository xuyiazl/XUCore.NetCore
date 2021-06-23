using System.IO;

namespace XUCore.NetCore.Oss
{
    public interface IOssMultiPartClient
    {
        void Create(string relativePath);
        (bool res, string url) Done();
        bool Upload(Stream stream, long size, int number);
    }
}