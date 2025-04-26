using System.Collections.Generic;
using Data;
using Interface;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.SaveLoad
{
    public class LoadSaveService:ISaveLoadService
    {
        private  IDataPersistence _dataPersistence;

        public GameData GameData;

        public GameData Load()
        {
            if (_dataPersistence == null)
            {
                Debug.LogError("Data persistence is not initialized!");
                return new GameData();
            }

            var loadedData = _dataPersistence.Load();
            GameData = loadedData ?? new GameData(); // Если null, создаём новый объект

            string json = JsonConvert.SerializeObject(GameData, Formatting.Indented);
            Debug.Log($"Loaded game data:\n{json}");

            return GameData;
        }
        public  LoadSaveService()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                _dataPersistence = new CloudDataPersistence(); // WebGL → в облако
            else
                _dataPersistence = new LocalDataPersistence(); // Остальные → локально
            
            if (_dataPersistence == null)
                Debug.LogError("Ошибка! _dataPersistence не был инициализирован.");
        }
        
        public void UpdateLocationProgressData(List<LocationProgressData> locationProgressData)
        {
            if (GameData == null)
            {
                Debug.LogError("GameData is null! Cannot update location progress data.");
                return;
            }

            GameData.LocationProgressData = locationProgressData;
            Debug.Log("GameData location progress data updated.");
        }

        public GameData GetGameData()
        {
            return GameData;
        }

        public void Save()
        {
            
            if (GameData == null)
            {
                Debug.LogWarning("GameData is null, trying to load existing data...");
                Load(); // Загружаем данные перед сохранением
            }

            if (GameData == null)
            {
                Debug.LogError("Failed to load GameData, aborting save.");
                return;
            }

            string json = JsonConvert.SerializeObject(GameData, Formatting.Indented);
            Debug.Log($"Saving game data:\n{json}");

            _dataPersistence.Save(GameData);
            Debug.Log("Game data saved.");
        }



        public void ChangeFirstStart()
        {
            if (GameData == null)
            {
                Debug.LogError("Game data is null! Cannot change first start flag.");
                return;
            }

            GameData.ChangeIsFirstStart();
            Save();  // Сразу сохраняем изменения
        }
        
        public void Reset() => _dataPersistence.Reset();

        public void Initialize(ISerializationService jsonSerializationService)
        {
            throw new System.NotImplementedException();
        }
    }
}