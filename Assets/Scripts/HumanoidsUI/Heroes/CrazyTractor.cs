using HumanoidsUI.AbstractLevel;

namespace HumanoidsUI.Heroes
{
    public class CrazyTractor : Hero
    {
        private const int Level = 9;
        
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            throw new System.NotImplementedException();

    }
}