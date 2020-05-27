namespace XUCore.Connections
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/12/25 22:10:44
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// 读写连接池
    /// </summary>
    public class ConnectionPool : IConnectionPool
    {
        private List<EndPoint> ReadWriteHosts { get; set; }
        private List<EndPoint> ReadOnlyHosts { get; set; }

        private IClient[] writeClients = new IClient[0];
        protected int WritePoolIndex;

        private IClient[] readClients = new IClient[0];
        protected int ReadPoolIndex;

        protected int RedisClientCounter = 0;
        protected int PoolSizeMultiplier = 10;

        public IClientFactory ClientFactory { get; set; }

        public int MaxReadPoolSize { get; set; }
        public int MaxWritePoolSize { get; set; }

        public ConnectionPool(IEnumerable<string> readHosts)
            : this(readHosts, readHosts)
        { }

        public ConnectionPool(IEnumerable<string> readHosts, int poolSizeMultiplier)
            : this(readHosts, readHosts, poolSizeMultiplier)
        { }

        public ConnectionPool(
            IEnumerable<string> readWriteHosts,
            IEnumerable<string> readOnlyHosts, int poolSizeMultiplier = 10)
        {
            PoolSizeMultiplier = poolSizeMultiplier;

            ReadWriteHosts = Utils.ToIpEndPoints(readWriteHosts);
            ReadOnlyHosts = Utils.ToIpEndPoints(readOnlyHosts);

            MaxWritePoolSize = ReadWriteHosts.Count * PoolSizeMultiplier;
            MaxReadPoolSize = ReadOnlyHosts.Count * PoolSizeMultiplier;

            writeClients = new IClient[MaxWritePoolSize];
            WritePoolIndex = 0;

            readClients = new IClient[MaxReadPoolSize];
            ReadPoolIndex = 0;
        }

        public void Release(IClient conn)
        {
            Dispose(conn);
        }

        public T GetClient<T>()
        {
            lock (writeClients)
            {
                AssertValidReadWritePool();

                IClient inActiveClient;
                while ((inActiveClient = GetInActiveWriteClient()) == null)
                {
                    Monitor.Wait(writeClients);
                }

                if (WritePoolIndex >= MaxWritePoolSize)
                    WritePoolIndex = 0;

                WritePoolIndex++;

                return (T)inActiveClient;
            }
        }

        private IClient GetInActiveWriteClient()
        {
            for (var i = 0; i < writeClients.Length; i++)
            {
                var nextIndex = (WritePoolIndex + i) % writeClients.Length;

                //Initialize if not exists
                var existingClient = writeClients[nextIndex];
                if (existingClient == null)
                {
                    if (existingClient != null)
                    {
                        existingClient.Dispose();
                    }

                    var nextHost = ReadWriteHosts[nextIndex % ReadWriteHosts.Count];

                    var client = ClientFactory.Create(
                        nextHost.Host, nextHost.Port);

                    client.Id = RedisClientCounter++;

                    writeClients[nextIndex] = client;

                    return client;
                }

                return writeClients[nextIndex];
            }
            return null;
        }

        public T GetReadOnlyClient<T>()
        {
            lock (readClients)
            {
                AssertValidReadOnlyPool();

                IClient inActiveClient;
                while ((inActiveClient = GetInActiveReadClient()) == null)
                {
                    Monitor.Wait(readClients);
                }

                if (ReadPoolIndex >= MaxReadPoolSize)
                    ReadPoolIndex = 0;

                ReadPoolIndex++;

                return (T)inActiveClient;
            }
        }

        private IClient GetInActiveReadClient()
        {
            for (var i = 0; i < readClients.Length; i++)
            {
                var nextIndex = (ReadPoolIndex + i) % readClients.Length;

                //Initialize if not exists
                var existingClient = readClients[nextIndex];
                if (existingClient == null)
                {
                    if (existingClient != null)
                    {
                        existingClient.Dispose();
                    }

                    var nextHost = ReadOnlyHosts[nextIndex % ReadOnlyHosts.Count];
                    var client = ClientFactory.Create(
                        nextHost.Host, nextHost.Port);

                    readClients[nextIndex] = client;

                    return client;
                }

                return readClients[nextIndex];
            }
            return null;
        }

        private void AssertValidReadWritePool()
        {
            if (writeClients.Length < 1)
                throw new InvalidOperationException("Need a minimum read-write pool size of 1");
        }

        private void AssertValidReadOnlyPool()
        {
            if (readClients.Length < 1)
                throw new InvalidOperationException("Need a minimum read pool size of 1");
        }

        ~ConnectionPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
            }

            // get rid of unmanaged resources
            for (var i = 0; i < writeClients.Length; i++)
            {
                Dispose(writeClients[i]);
            }
            for (var i = 0; i < readClients.Length; i++)
            {
                Dispose(readClients[i]);
            }
        }

        protected void Dispose(IClient connection)
        {
            if (connection == null) return;
            try
            {
                connection.Dispose();
            }
            catch
            {
            }
        }
    }
}