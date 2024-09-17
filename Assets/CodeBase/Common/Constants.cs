using System.Collections.Generic;

namespace Common
{
    public static class Constants
    {
        public const float Zero = 0.0f;
        
        public  static readonly string Initial = nameof(Initial);
        public  static readonly string Menu = nameof(Menu);
        public  static readonly string Location = nameof(Location);
        
        public  static readonly string Leaderboard = "Leaderboard";
        public  static readonly string Price = nameof(Price);
        
        public const float TimeScaleStop = 0.0f;
        public const float TimeScaleResume = 1.0f;
        public const float TimeFirstScale = 0.5f;
        public const float TimeSecondScale = 1.0f;
        public const float TimeThirdScale = 1.5f;
        public const float TimeFourthScale = 3f;
        
        public const float Visible = 1f;
        public const float HalfVisible = 0.5f;
        public const float Invisible = 0f;
        
        public  const  int MoneyForReward = 100;

        static readonly List<string> Name = new List<string>()
        {
            "Soldier",
            "Sergeant",
            "Grenadier",
            "Scout",
            "Sniper",
            "Turret"
        };
        
        public static string GetName(int index) => Name[index];
    }
}