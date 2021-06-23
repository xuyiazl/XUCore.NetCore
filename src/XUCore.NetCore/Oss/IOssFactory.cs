using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Oss
{
    public interface IOssFactory
    {
        bool CreateClient(string name, OssOptions options);
        IOssClient GetClient(string name);
    }
}
