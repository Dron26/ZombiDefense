using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Interface
{
    public interface IResourceLoadService:IService
    {
        public T Load<T>(string path) 
            where T : Object;

        public List<T> LoadAll<T>(string folderPath)
            where T : Object;
    }
}