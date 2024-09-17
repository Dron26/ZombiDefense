using UnityEngine;

namespace CameraMain
{
    public class InputHandler
    {
        private Vector3 _touchStartPosition;
        private Vector3 _currentTouchPosition;
        private bool _isDragging = false;
        public Vector3 MoveDirection { get; private set; }
        private Vector3 _mouseStartPosition;
        private bool _isMouseDragging = false;

        public void HandleInput()
        {
            HandleTouchInput();
            HandleMouseInput();
            HandleKeyboardInput();
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

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(1)) 
            {
                _mouseStartPosition = Input.mousePosition;
                _isMouseDragging = true;
            }

            if (Input.GetMouseButtonUp(1)) 
            {
                _isMouseDragging = false;
            }

            if (_isMouseDragging)
            {
                Vector3 mouseDelta = Input.mousePosition - _mouseStartPosition;
                MoveDirection = new Vector3(mouseDelta.x, 0, mouseDelta.y); 
                MoveDirection.Normalize();
            }
            else
            {
                MoveDirection = Vector3.zero;
            }
        }

        private void HandleKeyboardInput()
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");

            if (moveDirection != Vector3.zero)
            {
                MoveDirection = moveDirection;
                _isDragging = false;
                return;
            }

            if (_isDragging)
            {
                // Преобразуем экранные координаты в мировые
                Vector3 worldStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(_touchStartPosition.x, _touchStartPosition.y, Camera.main.transform.position.y));
                Vector3 worldCurrentPosition = Camera.main.ScreenToWorldPoint(new Vector3(_currentTouchPosition.x, _currentTouchPosition.y, Camera.main.transform.position.y));

                MoveDirection = worldCurrentPosition - worldStartPosition;
            }
        }
    }
}