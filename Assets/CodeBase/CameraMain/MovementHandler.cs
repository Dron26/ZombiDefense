using UnityEngine;

namespace CameraMain
{
    public class MovementHandler
    {
        private readonly Transform _cameraTransform;
        private readonly float _moveSpeed;
        private readonly Vector2 _minBounds;
        private readonly Vector2 _maxBounds;

        public MovementHandler(Transform cameraTransform, float moveSpeed, Vector2 minBounds, Vector2 maxBounds)
        {
            _cameraTransform = cameraTransform;
            _moveSpeed = moveSpeed;
            _minBounds = minBounds;
            _maxBounds = maxBounds;
        }

        public void MoveCamera(Vector3 moveDirection)
        {
            if (moveDirection != Vector3.zero)
            {
                moveDirection.y = 0f;
                moveDirection.Normalize();

                Vector3 newPosition = _cameraTransform.position + moveDirection * _moveSpeed * Time.deltaTime;
                newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
                newPosition.z = Mathf.Clamp(newPosition.z, _minBounds.y, _maxBounds.y);

                MoveTo(newPosition);
            }
        }

        private void MoveTo(Vector3 position)
        {
            _cameraTransform.position = position;
        }
    }
}