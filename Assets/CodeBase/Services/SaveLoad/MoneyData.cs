namespace Services.SaveLoad
{
    public class MoneyData
    {
        public int TempMoney { get; private set; }
        public int AllAmountMoney { get; private set; }

        public void AddMoney(int amount)
        {
            TempMoney += amount;
            AllAmountMoney += amount;
        }

        public int FixTempMoneyState()
        {
            int fixedAmount = TempMoney;
            TempMoney = 0;
            return fixedAmount;
        }
    }
}