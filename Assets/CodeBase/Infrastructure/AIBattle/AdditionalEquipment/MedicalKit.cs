using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle.AdditionalEquipment
{
    public class MedicalKit : EquipmentItem
    {
         private int _recoveryRate;
        public override ItemType ItemType => ItemType.MedicalKit;
        public override void Initialize(ItemData itemData)
        {
            _recoveryRate = itemData.RecoveryRate;
        }

        public MedicalKit(int recoveryRate)
        {
            _recoveryRate = recoveryRate;
        }

        public int GetRecoveryRate() => _recoveryRate;
    }
}