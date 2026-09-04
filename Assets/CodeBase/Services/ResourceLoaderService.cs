using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Services
{
    public class ResourceLoaderService : IResourceLoadService
    {
        private readonly Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        public T Load<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (_cache.ContainsKey(path)) return _cache[path] as T;

            var obj = Resources.Load<T>(path);
            if (obj == null) return null;

            _cache[path] = obj;
            return obj;
        }

        public List<T> LoadAll<T>(string path) where T : Object
        {
            var objects = Resources.LoadAll<T>(path);
            var list = new List<T>();
            
            foreach (var obj in objects)
            {
                if (obj == null) continue;
                list.Add(obj);
                var cacheKey = path + "/" + obj.name;
                _cache[cacheKey] = obj;
            }
            
            return list;
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
        
        public void Preload(string path)
        {
            var obj = Resources.Load(path);
            if (obj != null) _cache[path] = obj;
        }
    }
}