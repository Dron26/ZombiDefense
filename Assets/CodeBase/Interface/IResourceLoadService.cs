using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Interface
{
    public interface IResourceLoadService : IService
    {
        T Load<T>(string path) where T : Object;
        List<T> LoadAll<T>(string path) where T : Object;
        void ClearCache();
        void Preload(string path);
    }
}