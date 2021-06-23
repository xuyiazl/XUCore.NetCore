using System.Collections.Concurrent;

namespace XUCore.NetCore.Oss
{
    public class OssFactory : IOssFactory
    {
        private readonly ConcurrentDictionary<string, IOssClient> container;

        public OssFactory()
        {
            container = new ConcurrentDictionary<string, IOssClient>();
        }

        public bool CreateClient(string name, OssOptions options)
        {
            if (!container.ContainsKey(name))
            {
                IOssClient client = new OssClient(options);

                return container.TryAdd(name, client);
            }
            return false;
        }

        public IOssClient GetClient(string name)
        {
            if (!container.ContainsKey(name))
                return null;
            return container[name];
        }

    }

}
