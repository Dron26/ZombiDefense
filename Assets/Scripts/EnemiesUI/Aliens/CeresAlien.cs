using EnemiesUI.AbstractEntity;

namespace EnemiesUI.Aliens
{
    public class CeresAlien : Alien
    {
        private  int _level = 2;

        public override int GetLevel() => 
            _level;
    }
}