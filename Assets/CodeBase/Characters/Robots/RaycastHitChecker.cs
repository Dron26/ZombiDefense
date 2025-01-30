using System.Collections.Generic;
using Services;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Characters.Robots
{
    public class RaycastHitChecker : MonoBehaviour
    {
        private PointerEventData _pointerEventData;
        private GraphicRaycaster _raycaster; 
        private EventSystem _eventSystem;
        public Vector3 Point { get; set; }
    
        public void Initialize()
        {
            _raycaster = AllServices.Container.Single<UIHandler>().GetRaycaster();
            _eventSystem = AllServices.Container.Single<UIHandler>().GetEventSystem();
        }

        public bool  CanGetRaycastHit()
        {
            bool canGetPoint = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            _raycaster.Raycast(_pointerEventData, results);
        
            if (results.Count == 0)
            {
                if (Physics.Raycast(ray, out RaycastHit hit)&&!hit.collider.CompareTag("Point")&&!hit.collider.CompareTag("PlayerUnit"))
                {
                    canGetPoint = true;
                    Point=hit.point;
                }
            }

            return canGetPoint;
        }
    }
}