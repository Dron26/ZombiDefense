namespace Observer
{
    public interface IObservableHumanoid
    {
        void AddObserver(IObserverByHumanoid observerByHumanoid);
        void RemoveObserver(IObserverByHumanoid observerByHumanoid);
        void NotifyObservers(object data);
    }
}
