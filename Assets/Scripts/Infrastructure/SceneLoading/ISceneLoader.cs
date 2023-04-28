using System;
using Service;

namespace Infrastructure.SceneLoading
{
    public interface ISceneLoader : IService
    {
        void Load(string sceneName, Action onLoaded = null);
    }
}