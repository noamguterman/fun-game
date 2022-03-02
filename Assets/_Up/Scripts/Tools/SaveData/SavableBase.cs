using System;
using System.Collections.Generic;
using System.IO;
using Assets._Scripts.Tools.Base;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets._Scripts.Tools.SaveData
{
    [Serializable]
    public class SavableBase : Singleton<SavableBase>
    {
        private const string FileName = "SaveData";
        private static readonly string SavePath = string.Format("{0}/{1}", Application.persistentDataPath, FileName);
        [JsonRequired] private Dictionary<string, object> _saveList = new Dictionary<string, object>();
        public SavableBase() { }

        static SavableBase()
        {
            instance = Init<SavableBase>();
        }

        public T GetObject<T>(string key)
        {
            if (!_saveList.ContainsKey(key)) return default(T);
            if (_saveList[key] is T) return (T)_saveList[key];
            return (T)(_saveList[key] = JsonConvert.DeserializeObject<T>(_saveList[key].ToString()));
        }

        public void SetObject(string key, object value)
        {
            _saveList[key] = value;
            SaveProgress();
        }

        public bool RemoveObject(string key)
        {
            return _saveList.Remove(key);
        }

        protected static T Init<T>() where T : new()
        {
            if (!File.Exists(SavePath)) return new T();
            using (var stream = new FileStream(SavePath, FileMode.Open))
            {
                var sr = new StreamReader(stream);
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }

        public void SaveProgress()
        {
            var data = JsonConvert.SerializeObject(this);
            using (var stream = new FileStream(SavePath, FileMode.Create))
            {
                var sw = new StreamWriter(stream);
                sw.Write(data);
                sw.Flush();
            }
        }

        protected void OnObjectSet(string key, object changedObject)
        {
            if (onObjectSet == null) return;
            onObjectSet(key, changedObject);
        }

        public event ObjectChangeArgs onObjectSet;
    }

    public delegate void ObjectChangeArgs(string key, object changedObject);
}
