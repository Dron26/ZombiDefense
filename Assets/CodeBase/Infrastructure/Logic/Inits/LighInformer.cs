using System;

namespace Infrastructure.Logic.Inits
{
    public static class LighInformer
    {
        public static bool HasLight { get; private set; }

        public static void SetLight(bool hasLight)
        {
            HasLight = hasLight;
        }
    }
}