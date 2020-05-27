namespace XUCore.Collection
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:42:31
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System.Collections.Generic;

    public class PriorityQueue<T> : ListMapping<int, T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PriorityQueue()
            : base()
        {
            HighestKey = int.MinValue;
        }

        #endregion Constructor

        #region Public Functions

        /// <summary>
        /// Peek at the next thing in the queue
        /// </summary>
        /// <returns>The next item in queue or default(T) if it is empty</returns>
        public virtual T Peek()
        {
            if (Items.ContainsKey(HighestKey))
                return Items[HighestKey][0];
            return default(T);
        }

        public override void Add(int Priority, List<T> Value)
        {
            if (Priority > HighestKey)
                HighestKey = Priority;
            base.Add(Priority, Value);
        }

        public override void Add(System.Collections.Generic.KeyValuePair<int, List<T>> item)
        {
            if (item.Key > HighestKey)
                HighestKey = item.Key;
            base.Add(item);
        }

        public override void Add(int Priority, T Value)
        {
            if (Priority > HighestKey)
                HighestKey = Priority;
            base.Add(Priority, Value);
        }

        /// <summary>
        /// Removes an item from the queue and returns it
        /// </summary>
        /// <returns>The next item in the queue</returns>
        public virtual T Pop()
        {
            T ReturnValue = default(T);
            if (Items.ContainsKey(HighestKey) && Items[HighestKey].Count > 0)
            {
                ReturnValue = Items[HighestKey][0];
                Remove(HighestKey, ReturnValue);
                if (!ContainsKey(HighestKey))
                {
                    HighestKey = int.MinValue;
                    foreach (int Key in Items.Keys)
                        if (Key > HighestKey)
                            HighestKey = Key;
                }
            }
            return ReturnValue;
        }

        #endregion Public Functions

        #region Protected Variables

        /// <summary>
        /// Highest value key
        /// </summary>
        protected virtual int HighestKey { get; set; }

        #endregion Protected Variables
    }
}