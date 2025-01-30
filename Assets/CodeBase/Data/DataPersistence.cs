using Interface;
using Newtonsoft.Json;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace Data
{
    public class DataPersistence : IDataPersistence
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

            AllServices.Container.Single<ICharacterHandler>().Reset();
            AllServices.Container.Single<ICurrencyHandler>().Reset();
            AllServices.Container.Single<IAchievementsHandler>().Reset();
            AllServices.Container.Single<IEnemyHandler>().Reset();
            AllServices.Container.Single<ILocationHandler>().Reset();

            Save();
        }

        public void OnGameStart() => _gameData.OnGameStart();

        public void OnGameEnd() => _gameData.OnGameEnd();
    }
}