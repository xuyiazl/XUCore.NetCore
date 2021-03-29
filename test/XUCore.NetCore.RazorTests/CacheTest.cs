using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Interceptor;

namespace XUCore.NetCore.RazorTests
{
    public class CacheTest: ICacheTest
    {
        [CacheInterceptor(Key = "test", CacheSeconds = CacheTime.Day1)]
        public string GetPermission()
        {
            return "test";
        }
    }

    public interface ICacheTest
    {
        string GetPermission();
    }
}
