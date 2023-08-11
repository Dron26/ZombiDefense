namespace Infrastructure.Observer
{
    public interface IObserverByHumanoid
    {
        void NotifyFromHumanoid(object data);
        void NotifySelection(bool isSelected);
    }
}
