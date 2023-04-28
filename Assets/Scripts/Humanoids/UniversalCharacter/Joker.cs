using Humanoids.AbstractLevel;

namespace Humanoids.UniversalCharacter
{
    public class Joker : Humanoid
    {
        private const int Level = 0;

        public override bool IsLife() => 
            false;
        public override int GetLevel() => 
            Level;

        public override int GetPrice() => 
            throw new System.NotImplementedException();

        public override int GetDamage() => 
            throw new System.NotImplementedException();
        
        public override void ApplyDamage(int getDamage) => 
            throw new System.NotImplementedException();

        public override int GetDamageDone() => 
            throw new System.NotImplementedException();

        public override int DamageReceived() => 
            throw new System.NotImplementedException();

        public override int TotalPoints() => 
            throw new System.NotImplementedException();
        
        public override float GetRangeAttack() => 
            throw new System.NotImplementedException();
        
    }
}