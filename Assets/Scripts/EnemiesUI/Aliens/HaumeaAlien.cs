using EnemiesUI.AbstractEntity;

namespace EnemiesUI.Aliens
{
    public class HaumeaAlien : Alien
    {
        private int Level = 3;

        public override int GetLevel() =>
            Level;
    }
}