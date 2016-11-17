using System.Collections;
using System.Collections.Generic;

namespace MonoServer.Components.StaticFiles
{
    public abstract class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public int Count { get { return _dictionary.Count; } }

        public bool IsReadOnly { get { return true; } }

        public ICollection<TKey> Keys { get { return _dictionary.Keys; } }

        public ICollection<TValue> Values { get { return _dictionary.Values; } }

        public virtual void Add(KeyValuePair<TKey, TValue> item) => _dictionary.Add(item);

        public virtual void Add(TKey key, TValue value) => _dictionary.Add(key, value);

        public virtual void Clear() => _dictionary.Clear();

        public virtual bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

        public virtual bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

        public virtual bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item);

        public virtual bool Remove(TKey key) => _dictionary.Remove(key);

        public virtual bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
        
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
    }
}
