using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Location
{
    class WorkPointGroup:MonoCache
    {
        public List<WorkPoint> WorkPoints=new();

        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                WorkPoint workPoint = transform.GetChild(i).GetComponent<WorkPoint>();
                
                if (workPoint != null)
                {
                    WorkPoints.Add(workPoint);
                }
            }
        }
    }
}
