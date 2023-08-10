using UnityEngine;

namespace CameraMain
{
    public class MultiInputMovement : MonoBehaviour
    {
        private Vector3 _touchStartPosition;
        private Vector3 _currentTouchPosition;
        private bool _isDragging = false;

        public float moveSpeed = 5f;
        public Vector2 minBounds;
        public Vector2 maxBounds;
        private Vector2 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            HandleTouchInput();
            HandleMouseAndKeyboardInput();
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _touchStartPosition = touch.position;
                        _isDragging = true;
                        break;

                    case TouchPhase.Moved:
                        _currentTouchPosition = touch.position;
                        break;

                    case TouchPhase.Ended:
                        _isDragging = false;
                        break;
                }
            }
        }

        private void HandleMouseAndKeyboardInput()
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");

            if (moveDirection != Vector3.zero)
            {
                _isDragging = false;
            }

            if (_isDragging || moveDirection != Vector3.zero)
            {
                if (_isDragging)
                {
                    moveDirection = _currentTouchPosition - _touchStartPosition;
                }

                moveDirection.y = 0f;
                moveDirection.Normalize();

                Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
                newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
                newPosition.z = Mathf.Clamp(newPosition.z, minBounds.y, maxBounds.y);

                MoveTo(newPosition);
            }
        }

        private void MoveTo(Vector3 position)
        {
            transform.position = position;
        }
    }
}