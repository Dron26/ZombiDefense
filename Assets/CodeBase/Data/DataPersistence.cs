using Interface;
using Newtonsoft.Json;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace Data
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

        public void Reset()
        {
            
            _gameData.EnemyData.ClearEnemies();
            _gameData.ChangeIsFirstStart();

            AllServices.Container.Single<CharacterHandler>().Reset();
            AllServices.Container.Single<CurrencyHandler>().Reset();
            AllServices.Container.Single<AchievementsHandler>().Reset();
            AllServices.Container.Single<UIHandler>();
            AllServices.Container.Single<AudioSettingsHandler>();
            AllServices.Container.Single<EnemyHandler>().Reset();
            AllServices.Container.Single<LocationHandler>().Reset();
            
            Save();
        }
        
        public void OnGameStart() => _gameData.OnGameStart();

        public void OnGameEnd() => _gameData.OnGameEnd();
    }
}