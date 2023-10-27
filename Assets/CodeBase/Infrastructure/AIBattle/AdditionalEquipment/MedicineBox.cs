using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.AIBattle.AdditionalEquipment
{
    public class MedicineBox:MonoCache
    {
        [SerializeField] private int _recoveryRate;

        public MedicineBox(int recoveryRate)
        {
            _recoveryRate=recoveryRate;
        }
        
        public int GetRecoveryRate()
        {
            return _recoveryRate;
        }

    }
}