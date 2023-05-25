using System;
using UnityEngine;

public class MultiInputMovement : MonoBehaviour
{
    private Vector3 touchStartPosition;
    private Vector3 currentTouchPosition;
    private bool isDragging = false;

    public float moveSpeed = 5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public Vector2 _startPosition;

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
                    touchStartPosition = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    currentTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
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
            isDragging = false;
        }

        if (isDragging || moveDirection != Vector3.zero)
        {
            if (isDragging)
            {
                moveDirection = currentTouchPosition - touchStartPosition;
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