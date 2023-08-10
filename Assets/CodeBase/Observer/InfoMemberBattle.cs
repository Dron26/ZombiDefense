using System;

namespace Observer
{
    [Serializable]
    public class InfoMemberBattle
    {
        public int DamageDone;
        public int DamageReceived;

        public InfoMemberBattle(int damageDone, int damageReceived)
        {
            DamageDone = damageDone;
            DamageReceived = damageReceived;
        }
    }
}