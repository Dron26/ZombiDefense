using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace CameraMain
{
    //[RequireComponent(typeof(InputHandler))]
    //[RequireComponent(typeof(MovementHandler))]
    public class MultiInputMovement : MonoCache
    {
        public float moveSpeed = 5f;
        public Vector2 minBounds;
        public Vector2 maxBounds;

        private Vector2 _startPosition;
        private InputHandler _inputHandler;
        private MovementHandler _movementHandler;

        private void Start()
        {
            _startPosition = transform.position;
            _inputHandler = new InputHandler();
            _movementHandler = new MovementHandler(transform, moveSpeed, minBounds, maxBounds);
        }

        private void Update()
        {
            _inputHandler.HandleInput();
            _movementHandler.MoveCamera(_inputHandler.MoveDirection);
        }

        public void Initialize(CameraData data)
        {
            minBounds = new Vector2(data.MinBoundsX, data.MinBoundsY);
            maxBounds = new Vector2(data.MaxBoundsX, data.MaxBoundsY);

            ZoomController controller = GetComponentInChildren<ZoomController>();
            if (controller != null)
            {
                controller.SetData(data);
            }
            else
            {
                Debug.LogWarning("ZoomController не найден в дочерних объектах.");
            }
        }
    }
}