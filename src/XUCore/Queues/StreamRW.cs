namespace XUCore.Queues
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/30 18:07:31
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal sealed class FileHeader
    {
        public int Position, Current, Count;
    }

    internal sealed class StringEntry
    {
        public string Value;
        public long Pos;
    }

    internal sealed class BinaryEntry
    {
        public byte[] Value;
        public long Pos;
    }

    /*////////////////////////////////////////////////////////////////////////////////////*/

    internal sealed class StreamRW : FileStream
    {
        public StreamRW(string fileName)
            : base(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
        {
        }

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        public void WriteFileHeader(int position, int current, int count)
        {
            long pos = Position;
            Seek(0, SeekOrigin.Begin);

            var by = BitConverter.GetBytes(position);
            Write(by, 0, 4);

            by = BitConverter.GetBytes(current);
            Write(by, 0, 4);

            by = BitConverter.GetBytes(count);
            Write(by, 0, 4);

            Seek(pos, SeekOrigin.Begin);
        }

        public FileHeader ReadFileHeader()
        {
            if (Position > 0) Seek(0, SeekOrigin.Begin);

            byte[] data = new byte[12];

            Read(data, 0, 12);

            FileHeader tmp = new FileHeader();
            tmp.Position = BitConverter.ToInt32(data, 0);
            tmp.Current = BitConverter.ToInt32(data, 4);
            tmp.Count = BitConverter.ToInt32(data, 8);

            return tmp;
        }

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        public void AppendStringItem(string item)
        {
            long pos = Position;

            Seek(0, SeekOrigin.End);

            var encoding = UnicodeEncoding.UTF8;
            byte[] tmp = encoding.GetBytes(item);

            var size = BitConverter.GetBytes(tmp.Length);

            Write(size, 0, size.Length);
            Write(tmp, 0, tmp.Length);

            Seek(pos, SeekOrigin.Begin);
        }

        public void FastAppendStringItem(string item)
        {
            var encoding = UnicodeEncoding.UTF8;
            byte[] tmp = encoding.GetBytes(item);

            var size = BitConverter.GetBytes(tmp.Length);

            Write(size, 0, size.Length);
            Write(tmp, 0, tmp.Length);
        }

        public void AppendBinaryItem(byte[] item)
        {
            long pos = Position;

            Seek(0, SeekOrigin.End);

            var size = BitConverter.GetBytes(item.Length);

            Write(size, 0, size.Length);
            Write(item, 0, item.Length);

            Seek(pos, SeekOrigin.Begin);
        }

        public void FastAppendStringItem(byte[] item)
        {
            var size = BitConverter.GetBytes(item.Length);

            Write(size, 0, size.Length);
            Write(item, 0, item.Length);
        }

        /*////////////////////////////////////////////////////////////////////////////////////////*/

        public StringEntry ReadStringEntry()
        {
            if (Position == Length) return null;

            StringEntry R = new StringEntry();
            R.Pos = Position;

            byte[] data = new byte[4];
            Read(data, 0, 4);
            int size = BitConverter.ToInt32(data, 0);

            data = new byte[size];
            Read(data, 0, data.Length);
            R.Value = ReadString(data);

            return R;
        }

        public BinaryEntry ReadBinaryEntry()
        {
            if (Position == Length) return null;

            BinaryEntry R = new BinaryEntry();
            R.Pos = Position;

            byte[] data = new byte[4];
            Read(data, 0, 4);
            int size = BitConverter.ToInt32(data, 0);

            data = new byte[size];
            Read(data, 0, data.Length);
            R.Value = data;

            return R;
        }

        public LinkedList<StringEntry> ReadAllString()
        {
            LinkedList<StringEntry> tmp = new LinkedList<StringEntry>();

            long pos = Position;

            Seek(12, SeekOrigin.Begin);

            while (Position < Length)
            {
                tmp.AddLast(ReadStringEntry());
            }

            Seek(pos, SeekOrigin.Begin);
            return tmp;
        }

        public LinkedList<BinaryEntry> ReadAllBinary()
        {
            LinkedList<BinaryEntry> tmp = new LinkedList<BinaryEntry>();

            long pos = Position;

            Seek(12, SeekOrigin.Begin);

            while (Position < Length)
                tmp.AddLast(ReadBinaryEntry());

            Seek(pos, SeekOrigin.Begin);
            return tmp;
        }

        public Queue<string> ReadAllStringInQueue()
        {
            Queue<string> tmp = new Queue<string>();

            long pos = Position;

            Seek(12, SeekOrigin.Begin);

            while (Position < Length)
            {
                var x = ReadStringEntry();
                tmp.Enqueue(x.Value);
            }

            Seek(pos, SeekOrigin.Begin);
            return tmp;
        }

        public Queue<byte[]> ReadAllBinaryInQueue()
        {
            Queue<byte[]> tmp = new Queue<byte[]>();

            long pos = Position;

            Seek(12, SeekOrigin.Begin);

            while (Position < Length)
            {
                var x = ReadBinaryEntry();
                tmp.Enqueue(x.Value);
            }

            Seek(pos, SeekOrigin.Begin);
            return tmp;
        }

        private string ReadString(byte[] bytes)
        {
            return UTF32Encoding.UTF8.GetString(bytes);
        }

        private string ReadString(byte[] bytes, int index, int count)
        {
            return UTF32Encoding.UTF8.GetString(bytes, index, count);
        }
    }
}