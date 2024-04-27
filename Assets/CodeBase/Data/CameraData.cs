using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine.Serialization;

namespace Data
{
    public class CameraData:MonoCache
    {
        public float MinBoundsX;
        public float MinBoundsY;
        public float MaxBoundsX;
        public float MaxBoundsY;
        public float MinZoomDistance;
        public float MaxZoomDistance;
        public bool IsDay;
    }
}