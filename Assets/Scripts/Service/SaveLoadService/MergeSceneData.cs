using System;

namespace Service.SaveLoadService
{
    [Serializable]
    public class MergeSceneData
    {
        public int LevelHumanoid;
        public int AmountHumanoid;

        public MergeSceneData(int levelHumanoid, int amountHumanoid)
        {
            LevelHumanoid = levelHumanoid;
            AmountHumanoid = amountHumanoid;
        }
    }
}