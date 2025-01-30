using System;
using System.IO;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Services.SaveLoad
{
    public class SaveLoadService : MonoCache, ISaveLoadService
    {
        private const string SaveFileName = "saveData.json";
        private  string _filePath;
        private GameData _gameData;
        private  ISerializationService _serializationService;

        public GameData GameData => _gameData;
        public void Initialize(ISerializationService serializationService)
        {
            _serializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
            _filePath = Path.Combine(Application.persistentDataPath, SaveFileName);
            Load();
        }

        public void Save()
        {
            try
            {
                var json = _serializationService.Serialize(_gameData);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка сохранения: {ex.Message}");
            }
        }

        public void Load()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    _gameData = _serializationService.Deserialize<GameData>(json);
                }
                else
                {
                    _gameData = new GameData();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка загрузки: {ex.Message}");
                _gameData = new GameData();
            }
        }

        public void Reset()
        {
            _gameData = new GameData();
            Save();
        }

        
    }
}