using EnemiesUI.AbstractEntity;

namespace EnemiesUI.Pigs
{
    public class BomberPig : Pig
    {
        private int Level = 2;

        public override int GetLevel() =>
            Level;
    }
}