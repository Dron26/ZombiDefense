using EnemiesUI.AbstractEntity;

namespace EnemiesUI.Pigs
{
    public class CriminalPig : Pig
    {
        private int Level = 2;

        public override int GetLevel() =>
            Level;
    }
}