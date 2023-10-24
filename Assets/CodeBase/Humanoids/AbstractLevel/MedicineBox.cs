using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine.UIElements;

namespace Humanoids.AbstractLevel
{
    public class MedicineBox:MonoCache
    {
        public int RecoveryRate { get; set; }
        
        public MedicineBox(int recoveryRate)
        {
            RecoveryRate=recoveryRate;
        }
        
    }
}