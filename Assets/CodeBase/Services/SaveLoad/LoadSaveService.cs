using Data;
using Interface;
using UnityEngine;

namespace Services.SaveLoad
{
    public class LoadSaveService:ISaveLoadService
    {
        private  IDataPersistence _dataPersistence;
        private GameData _gameData;

        public GameData GameData => _gameData;

        public  LoadSaveService()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                _dataPersistence = new CloudDataPersistence(); // WebGL → в облако
            else
                _dataPersistence = new LocalDataPersistence(); // Остальные → локально

            _gameData = _dataPersistence.Load();

            // Проверяем первый запуск
            _gameData.AddInitialMoney();  // Даем 100 монет на первый запуск
            Save();  // Сохраняем данные после первого запуска
        }

        public void Save()
        {
            _dataPersistence.Save(_gameData); 
        }

        public GameData Load() => _dataPersistence.Load();

        public void Reset() => _dataPersistence.Reset();

        public void Initialize(ISerializationService jsonSerializationService)
        {
            throw new System.NotImplementedException();
        }
    }
}