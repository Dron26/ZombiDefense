using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace Data
{
    public class CameraData:MonoCache
    {
        public float minBoundsX;
        public float minBoundsY;
        public float maxBoundsX;
        public float maxBoundsY;
        public float minZoomDistance;
        public float maxZoomDistance;
    }
}