using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.NetCore.RazorTests
{
    public class CacheTest: ICacheTest
    {
        [AspectCachePull(HashKey = "test", RefreshSeconds = CacheTime.Day1)]
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
