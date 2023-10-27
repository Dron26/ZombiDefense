using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace CameraMain
{
    public class ZoomController : MonoCache
    {
        public Camera mainCamera;
        public float zoomSpeed = 0.5f;
        public float mouseZoomSpeed = 30f;
        public float minZoomDistance;
        public float maxZoomDistance;

        private Vector2 initialTouchPosition;
        private float initialCameraDistance;

        protected override void UpdateCustom()
        {
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    HandleTouchBegan(touch1, touch2);
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    HandleTouchMoved(touch1, touch2);
                }
            }
            else if (IsMouseZoomEnabled())
            {
                float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * mouseZoomSpeed;
                float newCameraHeight = mainCamera.transform.position.y - zoomDelta;

                newCameraHeight = Mathf.Clamp(newCameraHeight, minZoomDistance, maxZoomDistance);

                ApplyZoom(newCameraHeight);
            }
        }

        private bool IsMouseZoomEnabled()
        {
            return Input.mousePresent && Input.mouseScrollDelta.y != 0;
        }

        private void HandleTouchBegan(Touch touch1, Touch touch2)
        {
            initialTouchPosition = touch2.position - touch1.position;
            initialCameraDistance = Vector2.Distance(touch1.position, touch2.position);
        }

        private void HandleTouchMoved(Touch touch1, Touch touch2)
        {
            Vector2 currentTouchPosition = touch2.position - touch1.position;
            float currentCameraDistance = Vector2.Distance(touch1.position, touch2.position);

            if (currentCameraDistance != initialCameraDistance)
            {
                float zoomDelta = (currentCameraDistance - initialCameraDistance) * zoomSpeed;
                float newCameraHeight = mainCamera.transform.position.y - zoomDelta;

                newCameraHeight = Mathf.Clamp(newCameraHeight, minZoomDistance, maxZoomDistance);

                ApplyZoom(newCameraHeight);

                initialTouchPosition = currentTouchPosition;
                initialCameraDistance = currentCameraDistance;
            }
        }

        private void ApplyZoom(float newCameraHeight)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            cameraPosition.y = newCameraHeight;
            mainCamera.transform.position = cameraPosition;
        }

        public void SetData(CameraData data)
        {
            minZoomDistance=data.minZoomDistance;
            maxZoomDistance=data.maxZoomDistance;
        }
    }
}
