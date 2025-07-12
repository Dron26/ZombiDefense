using Data;
using Interface;
using Newtonsoft.Json;
using UnityEngine;
using YG;

namespace Services.SaveLoad
{
    public class CloudDataPersistence : IDataPersistence
    {
        public void Save(GameData data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.None);
            YG2.saves.GameDataJson = json;
            YG2.SaveProgress();
            Debug.Log("Cloud save completed");
        }

        public GameData Load()
        {
            var json = YG2.saves.GameDataJson;

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Cloud data is empty. Creating new GameData.");
                return new GameData();
            }

            try
            {
                var data = JsonConvert.DeserializeObject<GameData>(json);
                Debug.Log("Cloud load successful");
                return data ?? new GameData();
            }
            catch
            {
                Debug.LogError("Cloud load failed. Data corrupted?");
                return new GameData();
            }
        }

        public void Reset()
        {
            YG2.saves.GameDataJson = "";
            YG2.SaveProgress();
        }
    }
}