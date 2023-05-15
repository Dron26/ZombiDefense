using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Location
{
    public class WorkPointGroup : MonoCache
    {
        [SerializeField] public List<float> persentUp=new();
        private  List<WorkPoint> _workPoints = new();
        public UnityAction<WorkPoint> OnSelectPointToMove;
        public UnityAction<WorkPoint> OnSelectedPoint;
        public UnityAction<WorkPoint> OnSelectedStartPoint;
        
        
        
        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                WorkPoint workPoint = transform.GetChild(i).GetComponent<WorkPoint>();
                
                if (workPoint != null)
                {
                    workPoint.OnSelected += OnSelected;
                    _workPoints.Add(workPoint);
                }
            }
        }

        private void OnSelected(WorkPoint workpoint)
        {
            OnSelectedPoint?.Invoke(workpoint);
        }

        public List<WorkPoint>  GetWorkPoints()
        {
            return new List<WorkPoint>(_workPoints);
        }
        
        private void OnUpLevel(int number)
        {
            _workPoints[number].UpLevel(persentUp[_workPoints[number].Level + 1]);
        }
    }
}
