using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XUCore.Helpers;
using XUCore.Serializer;

namespace XUCore
{
    public class JsonDocument : IDictionary<string, object>, IDictionary
    {
        private readonly List<string> _orderedKeys;
        private readonly Dictionary<string, object> _dictionary;
        private readonly IComparer<string> _keyComparer;

        public JsonDocument()
        {
            _dictionary = new Dictionary<string, object>();
            _orderedKeys = new List<String>();
        }

        public JsonDocument(IComparer<string> keyComparer)
            : this()
        {
            if (keyComparer == null)
                throw new ArgumentNullException("keyComparer");

            _keyComparer = keyComparer;
        }

        public JsonDocument(string key, object value)
            : this()
        {
            Add(key, value);
        }

        public JsonDocument(IEnumerable<KeyValuePair<string, object>> dictionary)
            : this()
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach (var entry in dictionary)
                Add(entry.Key, entry.Value);
        }

        public static JsonDocument Node
        {
            get
            {
                return new JsonDocument();
            }
        }

        public object this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        ICollection IDictionary.Keys
        {
            get { return _dictionary.Keys; }
        }

        ICollection IDictionary.Values
        {
            get { return _dictionary.Values; }
        }

        public ICollection<object> Values
        {
            get { return _dictionary.Values; }
        }

        public ICollection<string> Keys
        {
            get { return _orderedKeys.AsReadOnly(); }
        }

        public object Get(string key)
        {
            object item;
            return _dictionary.TryGetValue(key, out item) ? item : null;
        }

        public T Get<T>(string key)
        {
            return Conv.To<T>(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public JsonDocument Add(string key, object value)
        {
            _dictionary.Add(key, value);
            _orderedKeys.Add(key);//Relies on ArgumentException from above if key already exists.
            EnsureKeyOrdering();
            return this;
        }

        public JsonDocument Batch(string key, params JsonDocument[] value)
        {
            return Add(key, value);
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            Add(key, value);
        }

        public JsonDocument Set(string key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (!_orderedKeys.Contains(key))
                _orderedKeys.Add(key);

            _dictionary[key] = value;

            EnsureKeyOrdering();

            return this;
        }

        public void Insert(string key, object value, int position)
        {
            _dictionary.Add(key, value);//Relies on ArgumentException from above if key already exists.
            _orderedKeys.Insert(position, key);
            EnsureKeyOrdering();
        }

        public JsonDocument Prepend(string key, object value)
        {
            Insert(key, value, 0);
            return this;
        }

        public JsonDocument Merge(JsonDocument source)
        {
            if (source == null)
                return this;

            foreach (var key in source.Keys)
                this[key] = source[key];

            return this;
        }

        public bool Remove(string key)
        {
            _orderedKeys.Remove(key);
            return _dictionary.Remove(key);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        bool IDictionary.Contains(object key)
        {
            return _orderedKeys.Contains(Convert.ToString(key));
        }

        void IDictionary.Add(object key, object value)
        {
            Add(Convert.ToString(key), value);
        }

        public void Clear()
        {
            _dictionary.Clear();
            _orderedKeys.Clear();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_dictionary).GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            Remove(Convert.ToString(key));
        }

        object IDictionary.this[object key]
        {
            get { return Get(Convert.ToString(key)); }
            set { Set(Convert.ToString(key), value); }
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)_dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public void CopyTo(JsonDocument destinationDocument)
        {
            if (destinationDocument == null)
                throw new ArgumentNullException("destinationDocument");

            //Todo: Fix any accidental reordering issues.

            foreach (var key in _orderedKeys)
            {
                if (destinationDocument.ContainsKey(key))
                    destinationDocument.Remove(key);
                destinationDocument[key] = this[key];
            }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            var removed = ((ICollection<KeyValuePair<string, object>>)_dictionary).Remove(item);
            if (removed)
                _orderedKeys.Remove(item.Key);
            return removed;
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_dictionary).CopyTo(array, index);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return _orderedKeys; /* no special object is need since _orderedKeys is internal.*/ }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        public override bool Equals(object obj)
        {
            if (obj is JsonDocument)
                return Equals(obj as JsonDocument);
            return base.Equals(obj);
        }

        public bool Equals(JsonDocument document)
        {
            if (document == null)
                return false;
            if (_orderedKeys.Count != document._orderedKeys.Count)
                return false;
            return GetHashCode() == document.GetHashCode();
        }

        public override int GetHashCode()
        {
            var hash = 27;
            foreach (var key in _orderedKeys)
            {
                var valueHashCode = GetValueHashCode(this[key]);
                unchecked
                {
                    hash = (13 * hash) + key.GetHashCode();
                    hash = (13 * hash) + valueHashCode;
                }
            }
            return hash;
        }

        private int GetValueHashCode(object value)
        {
            if (value == null)
                return 0;
            return (value is Array) ? GetArrayHashcode((Array)value) : value.GetHashCode();
        }

        private int GetArrayHashcode(Array array)
        {
            var hash = 0;
            foreach (var value in array)
            {
                var valueHashCode = GetValueHashCode(value);
                unchecked
                {
                    hash = (13 * hash) + valueHashCode;
                }
            }
            return hash;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _orderedKeys.Select(orderedKey => new KeyValuePair<string, object>(orderedKey, _dictionary[orderedKey])).GetEnumerator();
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>(this);
        }

        public override string ToString()
        {
            return this.ToJson();
        }


        private void EnsureKeyOrdering()
        {
            if (_keyComparer == null)
                return;

            _orderedKeys.Sort(_keyComparer);
        }
    }
}
