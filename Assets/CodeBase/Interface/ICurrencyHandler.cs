using Services;

namespace Interface
{
    public interface  ICurrencyHandler:IService
    {
        int GetCurrentMoney();
        void AddMoney(int amount);
        int FixTemporaryMoneyState();
    }
}