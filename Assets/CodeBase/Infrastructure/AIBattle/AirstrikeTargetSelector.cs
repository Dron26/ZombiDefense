using UnityEngine;
using UnityEngine.EventSystems;

namespace Infrastructure.AIBattle
{
    public class AirstrikeTargetSelector : MonoBehaviour
    {
        [SerializeField] private AirstrikeController _airstrikeController;
        [SerializeField] private Camera _targetCamera;
        [SerializeField] private LayerMask _targetMask = ~0;

        public bool IsSelecting { get; private set; }

        public void StartSelection()
        {
            _airstrikeController.Refresh();
            IsSelecting = _airstrikeController.CanExecute;
        }

        public void CancelSelection()
        {
            IsSelecting = false;
        }

        private void Update()
        {
            if (!IsSelecting)
            {
                return;
            }

            if (TryGetPointerPosition(out Vector2 pointerPosition) &&
                TryGetTargetPosition(pointerPosition, out Vector3 targetPosition))
            {
                if (_airstrikeController.TryExecute(targetPosition))
                {
                    IsSelecting = false;
                }
            }
        }

        private bool TryGetPointerPosition(out Vector2 pointerPosition)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                pointerPosition = Input.GetTouch(0).position;
                return !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            }

            if (Input.GetMouseButtonDown(0))
            {
                pointerPosition = Input.mousePosition;
                return !EventSystem.current.IsPointerOverGameObject();
            }

            pointerPosition = default;
            return false;
        }

        private bool TryGetTargetPosition(Vector2 pointerPosition, out Vector3 targetPosition)
        {
            Ray ray = _targetCamera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _targetMask))
            {
                targetPosition = hit.point;
                return true;
            }

            targetPosition = default;
            return false;
        }
    }
}
