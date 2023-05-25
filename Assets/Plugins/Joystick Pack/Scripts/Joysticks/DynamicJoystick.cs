using UnityEngine;
using UnityEngine.EventSystems;

namespace Plugins.Joystick_Pack.Scripts.Joysticks
{
    public class DynamicJoystick : Joystick
    {
        public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }
        public float Magnitude{ get { return _magnitude; }set {} }

        private float _magnitude;

        [SerializeField] private float moveThreshold = 1;
        protected override void Start()
        {
            MoveThreshold = moveThreshold;
            base.Start();
            background.gameObject.SetActive(false);
            _magnitude = 0;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);

            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            _magnitude = magnitude;

            if (magnitude > moveThreshold)
            {
                Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
                background.anchoredPosition += difference;
            }
            base.HandleInput(magnitude, normalised, radius, cam);
        }
    }
}