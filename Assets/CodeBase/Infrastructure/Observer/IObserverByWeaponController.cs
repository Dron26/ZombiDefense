using Infrastructure.Logic.WeaponManagment;

namespace Infrastructure.Observer
{
    public interface IObserverByWeaponController
    {
        void NotifyFromWeaponController(Weapon weapon);
    }
}