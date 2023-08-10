using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace UI.HUD
{
    public class RaTextureUpdater:MonoCache
    {
        
        [SerializeField]private RenderTexture _renderTexture;
        [SerializeField]private Camera _camera;
    }
}