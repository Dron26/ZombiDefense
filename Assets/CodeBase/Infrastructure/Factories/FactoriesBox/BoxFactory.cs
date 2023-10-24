using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Service.SaveLoad;
using UnityEngine;

namespace Infrastructure.Factories.FactoriesBox
{
    public class BoxFactory:MonoCache
    {
        [SerializeField] private WeaponBox _weaponBox;
        [SerializeField] private MedicineBox _medicineBox;
        private SaveLoadService _saveLoadService;
        
        public WeaponBox CreateWeapon()
        {
            GameObject newBox = Instantiate(_weaponBox.gameObject);
            WeaponBox box = newBox.GetComponent<WeaponBox>();
            return box;
        }

        public MedicineBox CreateMedicine()
        {
            GameObject newBox = Instantiate(_medicineBox.gameObject);
            MedicineBox box = newBox.GetComponent<MedicineBox>();
            return box;
        }
        
        public void Initialize(SaveLoadService saveLoadService)
        {
            _saveLoadService=saveLoadService;
        }
    }
}