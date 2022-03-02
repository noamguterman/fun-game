using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Tools.SaveData
{
    [Serializable]
    public class SaveList
    {
        [SerializeField] private List<string> _keys = new List<string>();
        [SerializeField] private List<object> _values = new List<object>();

        public List<string> Keys { get { return _keys; } }
        public List<object> Values { get { return _values; } }
        public int Count
        {
            get { return _keys.Count; }
        }

        public object this[string key]
        {
            get
            {
                return _values[_keys.IndexOf(key)];
            }
            set
            {
                if (!_keys.Contains(key))
                {
                    _keys.Add(key);
                    _values.Add(value);
                }
                else
                {
                    _values[_keys.IndexOf(key)] = value;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return _keys.Contains(key);
        }
        public void Add(string key, object value)
        {
            _keys.Add(key);
            _values.Add(value);
        }
    }
}
