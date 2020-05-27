namespace XUCore.Queues
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/14 10:24:32
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 双向链表优先级队列
    /// <para>
    ///     《注意线程安全的问题》
    /// </para>
    /// </summary>
    public sealed class PriorityQueue : IEnumerable
    {
        private static object syncLock = new object();

        private const int _queuesCount = PriorityEnums.Level_9 - PriorityEnums.Level_0 + 1;

        private readonly LinkedList<IPriority>[] _queues = new LinkedList<IPriority>[_queuesCount];

        private int _workItemsCount;

        public PriorityQueue()
        {
            for (int i = 0; i < _queues.Length; ++i)
            {
                _queues[i] = new LinkedList<IPriority>();
            }
        }

        /// <summary>
        /// 入列
        /// </summary>
        /// <param name="workItem"></param>
        public void Enqueue(IPriority workItem)
        {
            lock (syncLock)
            {
                int queueIndex = _queuesCount - (int)workItem.Priority - 1;

                _queues[queueIndex].AddLast(workItem);
                ++_workItemsCount;
            }
        }

        /// <summary>
        /// 出列
        /// </summary>
        /// <returns></returns>
        public IPriority Dequeue()
        {
            lock (syncLock)
            {
                IPriority workItem = null;

                if (_workItemsCount > 0)
                {
                    int queueIndex = GetNextNonEmptyQueue(-1);
                    workItem = _queues[queueIndex].First.Value;
                    _queues[queueIndex].RemoveFirst();
                    --_workItemsCount;
                }

                return workItem;
            }
        }

        private int GetNextNonEmptyQueue(int queueIndex)
        {
            for (int i = queueIndex + 1; i < _queuesCount; ++i)
            {
                if (_queues[i].Count > 0)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 队列数量
        /// </summary>
        public int Count
        {
            get
            {
                lock (syncLock)
                {
                    return _workItemsCount;
                }
            }
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Clear()
        {
            lock (syncLock)
            {
                if (_workItemsCount > 0)
                {
                    foreach (LinkedList<IPriority> queue in _queues)
                    {
                        queue.Clear();
                    }
                    _workItemsCount = 0;
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new PriorityQueueEnumerator(this);
        }

        private class PriorityQueueEnumerator : IEnumerator
        {
            private readonly PriorityQueue _priorityQueue;
            private int _queueIndex;
            private IEnumerator _enumerator;

            public PriorityQueueEnumerator(PriorityQueue priorityQueue)
            {
                _priorityQueue = priorityQueue;
                _queueIndex = _priorityQueue.GetNextNonEmptyQueue(-1);
                if (_queueIndex >= 0)
                {
                    _enumerator = _priorityQueue._queues[_queueIndex].GetEnumerator();
                }
                else
                {
                    _enumerator = null;
                }
            }

            #region IEnumerator Members

            public void Reset()
            {
                _queueIndex = _priorityQueue.GetNextNonEmptyQueue(-1);
                if (_queueIndex >= 0)
                {
                    _enumerator = _priorityQueue._queues[_queueIndex].GetEnumerator();
                }
                else
                {
                    _enumerator = null;
                }
            }

            public object Current
            {
                get
                {
                    return _enumerator.Current;
                }
            }

            public bool MoveNext()
            {
                if (null == _enumerator)
                {
                    return false;
                }
                if (!_enumerator.MoveNext())
                {
                    _queueIndex = _priorityQueue.GetNextNonEmptyQueue(_queueIndex);
                    if (-1 == _queueIndex)
                    {
                        return false;
                    }
                    _enumerator = _priorityQueue._queues[_queueIndex].GetEnumerator();
                    _enumerator.MoveNext();
                    return true;
                }
                return true;
            }

            #endregion IEnumerator Members
        }
    }
}