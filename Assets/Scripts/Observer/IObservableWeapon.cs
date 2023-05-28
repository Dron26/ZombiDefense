using Infrastructure.WeaponManagment;

namespace Observer
{
    public interface IObservableWeapon
    {
        void AddObserver(IObserverByWeaponController observerByHumanoid);
        void RemoveObserver(IObserverByWeaponController observerByHumanoid);
        void NotifyObserverWeaponController(Weapon weapon);
    }
}