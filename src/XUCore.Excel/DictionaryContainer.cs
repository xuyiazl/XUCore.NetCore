using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Excel
{
    /// <summary>
    /// 字典容器（解决单个对象超过2G大小限制，以及字典自动扩容造成的翻倍占据内存的问题）
    /// </summary>
    public class DictionaryContainer : IDisposable
    {
        private int capacity; // 每个Dictionary的固定容量
        private List<Dictionary<string, object>> container;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cap">最大长度设置</param>
        public DictionaryContainer(int cap = 4000000)
        {
            capacity = cap;
            container = new List<Dictionary<string, object>>(cap);
            container.Add(new Dictionary<string, object>(cap));
        }

        /// <summary>
        /// 统计列表总长度
        /// </summary>
        /// <returns></returns>
        public long Count
        {
            get
            {
                int count = 0;
                foreach (var dic in container)
                    count += dic.Count;
                return count;
            }
        }
        /// <summary>
        /// 添加新数据，会自动创建新的Dictionary对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="p"></param>
        public void Add(string key, object p)
        {
            if (container.ElementAt(container.Count - 1).Count < capacity)
            {
                var dic = container.ElementAt(container.Count - 1);

                dic[key] = p;
            }
            else
            {
                var dic = new Dictionary<string, object>(capacity);
                container.Add(dic);

                dic[key] = p;
            }
        }
        /// <summary>
        /// 查询是否包含某个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            foreach (var dic in container)
            {
                if (dic.ContainsKey(key))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 获取某个数据的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return GetValue(key); }
            set { Add(key, value); }
        }
        /// <summary>
        /// 获取某个数据的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            foreach (var dic in container)
            {
                if (dic.ContainsKey(key))
                    return dic[key];
            }
            return null;
        }
        /// <summary>
        /// 清空容器
        /// </summary>
        public void Clear()
        {
            container.Clear();
            container.Add(new Dictionary<string, object>(capacity));
        }

        public void Dispose()
        {
            container.Clear();
        }
    }
}
