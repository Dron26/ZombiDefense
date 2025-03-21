using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Location
{
    public class WorkPointGroup : MonoCache
    {
        public int persentUp=5;
        private  List<WorkPoint> _workPoints = new();
        private  List<int> _workPointsPercent = new();
        public UnityAction<WorkPoint> OnSelectPointToMove;
        public UnityAction<WorkPoint> OnSelectedPoint;
        public UnityAction<WorkPoint> OnSelectedStartPoint;
        private IUpgradeTree _upgradeTree;
        public void Initialize()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                WorkPoint workPoint = transform.GetChild(i).GetComponent<WorkPoint>();
                
                if (workPoint != null)
                {
                    workPoint.OnSelected += OnSelected;
                    workPoint.Initialize(i);
                    _workPoints.Add(workPoint);
                }
            }
            
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            SetUpgrades();
        }

        public void OnSelected(WorkPoint workpoint)
        {
            OnSelectedPoint?.Invoke(workpoint);
        }

        public List<WorkPoint>  GetWorkPoints()
        {
            return new List<WorkPoint>(_workPoints);
        }
        
        public void UpLevel(WorkPoint workPoint)
        {
            int index = _workPoints.IndexOf(workPoint);
            
            if (index!=-1)
            {
                workPoint.UpLevel(persentUp*workPoint.Level);
                print("Up Level " + workPoint.Level);
            }
            else
            {
                print("Error: WorkPoint not found");
            }
        }

        private void SetUpgrades()
        {
            float value=_upgradeTree.GetUpgradeValue(UpgradeGroupType.Defence,UpgradeType.IncreaseDefensePoint)[0];

            for (int i = 0; i < _workPoints.Count; i++)
            {
                _workPoints[i].SetDefenceValue(value); 
            }
        }
    }
}