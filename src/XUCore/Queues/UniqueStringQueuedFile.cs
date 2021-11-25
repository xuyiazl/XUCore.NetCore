﻿namespace XUCore.Queues
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 18:08:30
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.IO;

    public class UniqueStringQueuedFile : IDisposable
    {
        private StreamRW dataFile;

        private bool CaseSensitive;
        private string FileName;
        private int count, current, realCurrent;
        private int cursor, realCursor;
        private object SyObject;
        private Queue<string> AllItems;

        public int Count { get { return count; } }
        public int Current { get { return current; } }
        public int RealCurrent { get { return realCurrent; } }
        public long Cursor { get { return cursor; } }
        public long RealCursor { get { return realCursor; } }

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        #region Ctor

        public UniqueStringQueuedFile(string fileName)
            : this(fileName, false)
        {
        }

        public UniqueStringQueuedFile(string fileName, bool caseSensitive)
        {
            CaseSensitive = caseSensitive;
            FileName = fileName;
            dataFile = new StreamRW(fileName);
            SyObject = new object();

            if (dataFile.Length == 0)
            {
                dataFile.WriteFileHeader(0, 0, 0);
                count = current = 0;
                dataFile.Seek(0, SeekOrigin.End);
                realCursor = cursor = (int)dataFile.Position;
                AllItems = new Queue<string>();
            }
            else
            {
                var tmp = dataFile.ReadFileHeader();
                count = tmp.Count;
                realCurrent = current = tmp.Current;
                realCursor = cursor = tmp.Position;

                AllItems = dataFile.ReadAllStringInQueue();

                dataFile.Seek(cursor, SeekOrigin.Begin);
            }
        }

        public static UniqueStringQueuedFile CreateFile(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            return new UniqueStringQueuedFile(fileName, false);
        }

        public static UniqueStringQueuedFile CreateFile(string fileName, string[] items)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var tmp = new UniqueStringQueuedFile(fileName, false);

            for (int i = 0; i < items.Length; i++)
                tmp.Enqueue(items[i]);

            return tmp;
        }

        #endregion Ctor

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        public void Enqueue(string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            var testStr = CaseSensitive == true ? str : str.ToLower();

            lock (SyObject)
            {
                if (AllItems.Contains(testStr)) return;
                AllItems.Enqueue(testStr);

                dataFile.AppendStringItem(str);
                count++;
                dataFile.WriteFileHeader(cursor, current, count);
            }
        }

        public void Enqueue(List<string> str)
        {
            var tmp = new List<string>();

            lock (SyObject)
            {
                try
                {
                    for (int i = 0; i < str.Count; i++)
                    {
                        var testStr = CaseSensitive == true ? str[i] : str[i].ToLower();

                        if (!AllItems.Contains(testStr))
                            tmp.Add(str[i]);
                    }
                }
                catch (Exception)
                {
                }

                if (tmp.Count > 0)
                {
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        var testStr = CaseSensitive == true ? tmp[i] : tmp[i].ToLower();

                        dataFile.AppendStringItem(tmp[i]);
                        AllItems.Enqueue(testStr);
                    }

                    count += tmp.Count;

                    dataFile.WriteFileHeader(cursor, current, count);
                }
            }
        }

        public List<string> Dequeue(int n)
        {
            lock (this.SyObject)
            {
                UpdateState();

                var R = new List<string>(n);
                int Index = 0;

                while (true)
                {
                    var tmp = dataFile.ReadStringEntry();
                    if (tmp == null) return R;

                    R.Add(tmp.Value);
                    ++Index;
                    realCurrent++;
                    realCursor = (int)dataFile.Position;

                    if (Index == n)
                        break;
                }

                return R;
            }
        }

        public List<string> ShadowDequeueAll()
        {
            lock (this.SyObject)
            {
                var R = new List<string>();
                using var tmpFile = new StreamRW(FileName);
                tmpFile.Seek(12, SeekOrigin.Begin);

                while (true)
                {
                    var E = tmpFile.ReadStringEntry();

                    if (E == null) return R;

                    R.Add(E.Value);
                }
                return R;
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

        #region IDisposable Members

        public void Dispose() => dataFile.Dispose();

        #endregion IDisposable Members
    }
}