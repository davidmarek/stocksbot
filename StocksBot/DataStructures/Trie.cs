using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace StocksBot.DataStructures
{
    public class Trie<TValue> : IDictionary<string, TValue>
    {
        private class TrieNode
        {
            private bool containsValue = false;
            private TValue nodeValue;
            private Lazy<ConcurrentDictionary<char, TrieNode>> nodes = new Lazy<ConcurrentDictionary<char, TrieNode>>(true);

            public TValue Value {
                get
                {
                    if (this.containsValue)
                        return this.nodeValue;
                    else
                        throw new KeyNotFoundException();
                }
                set
                {
                    this.nodeValue = value;
                    this.containsValue = true;
                }
            }

            public bool ContainsKey(char key)
            {
                return this.nodes.IsValueCreated && this.nodes.Value.ContainsKey(key);
            }

            public bool ContainsValue()
            {
                return this.containsValue;
            }

            public TrieNode this[char key]
            {
                get => this.nodes.Value.GetOrAdd(key, _ => new TrieNode());
                set => this.nodes.Value.TryAdd(key, value);
            }
        }

        private Lazy<TrieNode> root = new Lazy<TrieNode>(true);

        public TValue this[string key] {
            get {
                if (this.TryGetValue(key, out TValue value))
                    return value;
                else
                    throw new KeyNotFoundException();
            }
            set => this.Add(key, value);
        }

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => false;

        public void Add(string key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var node = this.root.Value;
            foreach (var c in key)
            {
                node = node[c];
            }

            node.Value = value;
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.root = new Lazy<TrieNode>(true);
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            TValue foundValue;
            if (!this.TryGetValue(item.Key, out foundValue))
                return false;

            return foundValue.Equals(item.Value);
        }

        public bool ContainsKey(string key)
        {
            if (!this.root.IsValueCreated)
                return false;

            var node = this.root.Value;
            foreach (var c in key)
            {
                if (!node.ContainsKey(c))
                    return false;

                node = node[c];
            }

            return node.ContainsValue();
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out TValue value)
        {
            value = default(TValue);
            if (!this.root.IsValueCreated)
                return false;

            var node = this.root.Value;
            foreach (var c in key)
            {
                if (!node.ContainsKey(c))
                    return false;

                node = node[c];
            }

            if (node.ContainsValue())
            {
                value = node.Value;
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
