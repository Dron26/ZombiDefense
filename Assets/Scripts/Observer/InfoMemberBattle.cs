using System;

namespace Observer
{
    [Serializable]
    public class InfoMemberBattle
    {
        public int DamageDone;
        public int DamageReceived;
        public int TotalPoints;

        public InfoMemberBattle(int damageDone, int damageReceived, int totalPoints)
        {
            DamageDone = damageDone;
            DamageReceived = damageReceived;
            TotalPoints = totalPoints;
        }
    }
}