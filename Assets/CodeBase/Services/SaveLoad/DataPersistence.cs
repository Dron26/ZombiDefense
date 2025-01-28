using Data;
using Interface;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.SaveLoad
{
    public class DataPersistence:IDataPersistence
    {
        
        private const string Key = "Key";
        private GameData _gameData;

        public DataPersistence()
        {
            if (PlayerPrefs.HasKey(Key))
                _gameData = JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(Key));
            else
                _gameData = new GameData();
        }

        public void Save()
        {
            var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            PlayerPrefs.SetString(Key, JsonConvert.SerializeObject(_gameData, Formatting.Indented, settings));
            PlayerPrefs.Save();
        }

        public GameData LoadData() => _gameData;

        public void ClearData()
        {
            _gameData.ClearSpawnLocationData();
            Save();
        }

        public void ResetProgress()
        {
            _gameData = new GameData();
            SetFirstStart();
        }

        public void SetFirstStart()
        {
            _gameData.ChangeIsFirstStart();
            Save();
        }

        public void OnGameStart() => _gameData.OnGameStart();

        public void OnGameEnd() => _gameData.OnGameEnd();
    }
}