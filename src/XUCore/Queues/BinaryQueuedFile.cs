namespace XUCore.Queues
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 18:07:10
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinaryQueuedFile : IDisposable
    {
        private StreamRW dataFile;

        private string FileName;
        private int count, current, realCurrent;
        private int cursor, realCursor;
        private object SyObject;
        private BinaryFormatter mSerialize;

        public int Count { get { return count; } }
        public int Current { get { return current; } }
        public int RealCurrent { get { return realCurrent; } }
        public long Cursor { get { return cursor; } }
        public long RealCursor { get { return realCursor; } }

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        #region Ctor

        public BinaryQueuedFile(string fileName)
        {
            mSerialize = new BinaryFormatter();
            FileName = fileName;
            dataFile = new StreamRW(fileName);
            SyObject = new object();

            if (dataFile.Length == 0)
            {
                dataFile.WriteFileHeader(0, 0, 0);
                count = current = 0;
                dataFile.Seek(0, SeekOrigin.End);
                realCursor = cursor = (int)dataFile.Position;
            }
            else
            {
                var tmp = dataFile.ReadFileHeader();
                count = tmp.Count;
                realCurrent = current = tmp.Current;
                realCursor = cursor = tmp.Position;

                dataFile.Seek(cursor, SeekOrigin.Begin);
            }
        }

        public static BinaryQueuedFile CreateFile(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            return new BinaryQueuedFile(fileName);
        }

        public static BinaryQueuedFile CreateFile(string fileName, string[] items)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var tmp = new BinaryQueuedFile(fileName);

            for (int i = 0; i < items.Length; i++)
                tmp.Enqueue(items[i]);

            return tmp;
        }

        #endregion Ctor

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        public void Enqueue(object str)
        {
            lock (SyObject)
            {
                dataFile.AppendBinaryItem(Serialize(str));

                count++;
                dataFile.WriteFileHeader(cursor, current, count);
            }
        }

        public void Enqueue(object[] str)
        {
            lock (SyObject)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    var tmp = Serialize(str[i]);

                    dataFile.AppendBinaryItem(tmp);
                }

                count += str.Length;
                dataFile.WriteFileHeader(cursor, current, count);
            }
        }

        public void Enqueue(List<object> str)
        {
            lock (SyObject)
            {
                for (int i = 0; i < str.Count; i++)
                {
                    var tmp = Serialize(str[i]);

                    dataFile.AppendBinaryItem(tmp);
                }

                count += str.Count;
                dataFile.WriteFileHeader(cursor, current, count);
            }
        }

        public List<object> Dequeue(int n)
        {
            lock (this.SyObject)
            {
                UpdateState();

                List<object> R = new List<object>(n);
                int Index = 0;

                while (true)
                {
                    var tmp = dataFile.ReadBinaryEntry();
                    if (tmp == null) return R;

                    R.Add(Deserialize(tmp.Value));
                    ++Index;
                    realCurrent++;
                    realCursor = (int)dataFile.Position;

                    if (Index == n)
                        break;
                }

                return R;
            }
        }

        public List<object> ShadowDequeueAll()
        {
            lock (this.SyObject)
            {
                List<object> R = new List<object>();
                using (var tmpFile = new StreamRW(FileName))
                {
                    tmpFile.Seek(12, SeekOrigin.Begin);

                    while (true)
                    {
                        var E = tmpFile.ReadBinaryEntry();

                        if (E == null) return R;

                        R.Add(Deserialize(E.Value));
                    }
                    return R;
                }
            }
        }

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        public void UpdateState()
        {
            if (realCursor != cursor)
            {
                cursor = realCursor;
                current = realCurrent;

                dataFile.WriteFileHeader(cursor, current, count);
            }
        }

        private byte[] Serialize(object obj)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                mSerialize.Serialize(mem, obj);

                byte[] Bs = new byte[mem.Length];

                mem.Seek(0, SeekOrigin.Begin);
                mem.Read(Bs, 0, Bs.Length);
                return Bs;
            }
        }

        private object Deserialize(byte[] bytes)
        {
            using (MemoryStream mem = new MemoryStream(bytes))
                return mSerialize.Deserialize(mem);
        }

        #region IDisposable Members

        public void Dispose()
        {
            dataFile.Dispose();
        }

        #endregion IDisposable Members
    }
}