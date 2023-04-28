using HumanoidsUI.AbstractLevel;

namespace HumanoidsUI.UniversalCharacter
{
    public class Joker : HumanoidUI
    {
        private const int Level = 0;
        public override int GetLevel() => 
            Level;

        public override int GetPrice() => 
            throw new System.NotImplementedException();
        
    }
}