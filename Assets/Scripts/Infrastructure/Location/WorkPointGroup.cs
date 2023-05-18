using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
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
        private SaveLoad _saveLoad;
        
        
        private void TakeAllWorkPoints()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                WorkPoint workPoint = transform.GetChild(i).GetComponent<WorkPoint>();
                
                if (workPoint != null)
                {
                    workPoint.OnSelected += OnSelected;
                    workPoint.SetSaveLoad(_saveLoad);
                    _workPoints.Add(workPoint);
                }
            }
        }

        public void OnSelected(WorkPoint workpoint)
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


        public void Initialize(SaveLoad saveLoad)
        {
            _saveLoad=saveLoad;
            TakeAllWorkPoints();
        }
    }
}
