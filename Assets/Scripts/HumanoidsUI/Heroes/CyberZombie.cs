using HumanoidsUI.AbstractLevel;

namespace HumanoidsUI.Heroes
{
    public class CyberZombie : Hero
    {
        private const int Level = 10;
        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            throw new System.NotImplementedException();
        
    }
}