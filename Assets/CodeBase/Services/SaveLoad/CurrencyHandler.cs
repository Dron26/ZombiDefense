using Data;
using Interface;

namespace Services.SaveLoad
{
    public class CurrencyHandler:ICurrencyHandler
    {
        private readonly MoneyData _moneyData;

        public CurrencyHandler(MoneyData moneyData)
        {
            _moneyData = moneyData;
        }

        public int GetCurrentMoney() => _moneyData.AllAmountMoney;

        public void AddMoney(int amount) => 
            _moneyData.AddMoney(amount);

        public int FixTemporaryMoneyState() => 
            _moneyData.FixTempMoneyState();
    }
}