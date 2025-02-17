using Data;
using Interface;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.SaveLoad
{
    public class LoadSaveService:ISaveLoadService
    {
        private  IDataPersistence _dataPersistence;
        private GameData _gameData;

        public GameData GameData => _gameData;

        public GameData Load()
        {
            var loadedData = _dataPersistence.Load();
            _gameData=loadedData;

            string json = JsonConvert.SerializeObject(loadedData, Formatting.Indented);
            Debug.Log($"Loaded game data:\n{json}");

            return loadedData;
        }
        public  LoadSaveService()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                _dataPersistence = new CloudDataPersistence(); // WebGL → в облако
            else
                _dataPersistence = new LocalDataPersistence(); // Остальные → локально
        }

        public void Save()
        {
            if (_gameData == null)
            {
                Debug.LogError("Game data is null!");
                return;
            }

            // Сериализация данных в JSON
            string json = JsonConvert.SerializeObject(_gameData, Formatting.Indented);  // Добавляем отступы для лучшей читаемости
            Debug.Log($"Saving game data:\n{json}");  // Логируем, что сохраняется

            _dataPersistence.Save(_gameData);  // Сохраняем данные

            Debug.Log("Game data saved.");
        }



        public void ChangeFirstStart()
        {
            _gameData.ChangeIsFirstStart();
        }
        
        public void Reset() => _dataPersistence.Reset();

        public void Initialize(ISerializationService jsonSerializationService)
        {
            throw new System.NotImplementedException();
        }
    }
}