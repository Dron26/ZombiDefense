using Infrastructure.WeaponManagment;

namespace Observer
{
    public interface IObserverByWeaponController
    {
        void NotifyFromWeaponController(Weapon weapon);
    }
}