using Infrastructure.BaseMonoCache.Code.MonoCache;
using YG;

namespace Services
{
    public class WebStateController:MonoCache
    {
        private void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus)
                YG2.GameplayStart();
            else
                YG2.GameplayStop();
        }

        
        private void OnApplicationQuit()
        {
            // Send end session event for analytics before quitting
           // AnalyticService.Instance.SendEvent(EventName.endSession);
        }

        /// <summary>
        /// Called when this object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Send end session event for analytics on destruction as well
           // AnalyticService.Instance.SendEvent(EventName.endSession);
        }
    }
}