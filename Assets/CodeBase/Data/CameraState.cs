using UnityEngine;

namespace Data
{
    public class CameraState
    {
        private Camera _cameraPhysical;
        private Camera _cameraUI;

        public Camera PhysicalCamera => _cameraPhysical;
        public Camera UICamera => _cameraUI;

        public void SetCameras(Camera cameraPhysical, Camera cameraUI)
        {
            _cameraPhysical = cameraPhysical;
            _cameraUI = cameraUI;
        }
    }
}