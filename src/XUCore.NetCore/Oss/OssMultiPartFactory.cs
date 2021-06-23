using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Oss
{
    public class OssMultiPartFactory : IOssMultiPartFactory
    {
        private readonly ConcurrentDictionary<string, OssOptions> container;
        private readonly ConcurrentDictionary<string, IOssMultiPartClient> clientContainer;

        public OssMultiPartFactory()
        {
            container = new ConcurrentDictionary<string, OssOptions>();
            clientContainer = new ConcurrentDictionary<string, IOssMultiPartClient>();
        }

        public bool CreateClient(string name, OssOptions options)
        {
            if (!container.ContainsKey(name))
            {
                return container.TryAdd(name, options);
            }
            return false;
        }

        public IOssMultiPartClient GetClient(string name, string token, string relativePath)
        {
            if (!clientContainer.ContainsKey(token))
            {
                if (!container.ContainsKey(name))
                    return null;

                var options = container[name];

                IOssMultiPartClient client = new OssMultiPartClient(options);

                client.Create(relativePath);

                clientContainer.TryAdd(name, client);

                return client;
            }

            return clientContainer[token];
        }

        public bool RemoveClient(string token)
        {
            if (clientContainer.ContainsKey(token))
            {
                IOssMultiPartClient s;
                return clientContainer.TryRemove(token, out s);
            }

            return false;
        }
    }
}
