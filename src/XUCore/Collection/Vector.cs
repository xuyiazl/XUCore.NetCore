namespace XUCore.Collection
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:45:02
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;

    public class Vector<T> : IList<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Vector()
        {
            DefaultSize = 2;
            Items = new T[DefaultSize];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InitialSize">Initial size of the vector</param>
        public Vector(int InitialSize)
        {
            if (InitialSize < 1) throw new ArgumentOutOfRangeException("InitialSize");
            DefaultSize = InitialSize;
            Items = new T[InitialSize];
        }

        #endregion Constructor

        #region IList<T> Members

        public virtual int IndexOf(T item)
        {
            return Array.IndexOf<T>(this.Items, item, 0, this.NumberItems);
        }

        public virtual void Insert(int index, T item)
        {
            if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");

            if (this.NumberItems == this.Items.Length)
                Array.Resize<T>(ref this.Items, this.Items.Length * 2);
            if (index < this.NumberItems)
                Array.Copy(this.Items, index, this.Items, index + 1, this.NumberItems - index);
            this.Items[index] = item;
            ++this.NumberItems;
        }

        public virtual void RemoveAt(int index)
        {
            if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");

            if (index < this.NumberItems)
                Array.Copy(this.Items, index + 1, this.Items, index, this.NumberItems - (index + 1));
            this.Items[this.NumberItems - 1] = default(T);
            --this.NumberItems;
        }

        public virtual T this[int index]
        {
            get
            {
                if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");
                return this.Items[index];
            }
            set
            {
                if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");
                this.Items[index] = value;
            }
        }

        #endregion IList<T> Members

        #region ICollection<T> Members

        public virtual void Add(T item)
        {
            Insert(this.NumberItems, item);
        }

        public virtual void Clear()
        {
            Array.Clear(this.Items, 0, this.Items.Length);
            this.NumberItems = 0;
        }

        public virtual bool Contains(T item)
        {
            return (this.IndexOf(item) >= 0);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.Items, 0, array, arrayIndex, this.NumberItems);
        }

        public virtual int Count
        {
            get { return this.NumberItems; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            int Index = this.IndexOf(item);
            if (Index >= 0)
            {
                this.RemoveAt(Index);
                return true;
            }
            return false;
        }

        #endregion ICollection<T> Members

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            for (int x = 0; x < this.NumberItems; ++x)
                yield return this.Items[x];
        }

        #endregion IEnumerable<T> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int x = 0; x < this.NumberItems; ++x)
                yield return this.Items[x];
        }

        #endregion IEnumerable Members

        #region Protected Variables/Properties

        /// <summary>
        /// Default size
        /// </summary>
        protected virtual int DefaultSize { get; set; }

        /// <summary>
        /// Internal list of items
        /// </summary>
        protected T[] Items = null;

        /// <summary>
        /// Number of items in the list
        /// </summary>
        protected virtual int NumberItems { get; set; }

        #endregion Protected Variables/Properties
    }
}