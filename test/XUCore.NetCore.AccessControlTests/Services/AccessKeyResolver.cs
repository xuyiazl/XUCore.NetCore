using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.NetCore.AccessControlTests.Services
{
    public class AccessKeyResolver
    {
        private readonly Dictionary<string, string> _accessKeys = new Dictionary<string, string>()
        {
            { "/Home/Test", "Abcd" },
        };

        public string GetAccessKey(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            return _accessKeys.ContainsKey(path) ? _accessKeys[path] : null;
        }
    }
}
