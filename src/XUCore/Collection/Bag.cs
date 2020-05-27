namespace XUCore.Collection
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:38:03
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class Bag<T> : ICollection<T>
    {
        public Bag()
        {
            Items = new ConcurrentDictionary<T, int>();
        }

        public virtual void Add(T item)
        {
            if (Items.ContainsKey(item))
                ++Items[item];
            else
                Items.TryAdd(item, 1);
        }

        public virtual void Clear()
        {
            Items.Clear();
        }

        public virtual bool Contains(T item)
        {
            return Items.ContainsKey(item);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Not used</param>
        /// <param name="arrayIndex">Not used</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public virtual int Count
        {
            get { return Items.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            return Items.TryRemove(item, out int value);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        public virtual int this[T index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        protected virtual ConcurrentDictionary<T, int> Items { get; set; }
    }
}