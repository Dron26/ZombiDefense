using System;
using Data;
using Interface;
using Newtonsoft.Json;
using UnityEngine;

public class LocalDataPersistence : IDataPersistence
{
    private const string Key = "GameData";  // Ключ для PlayerPrefs

    // Сохраняем данные в локальном хранилище (PlayerPrefs)
    public void Save(GameData data)
    {
        var json = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(Key, json);  // Сохраняем данные в PlayerPrefs
        PlayerPrefs.Save();  // Применяем изменения
    }

    // Загружаем данные из локального хранилища
    public GameData Load()
    {
        try
        {
            string json = PlayerPrefs.GetString(Key);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("No saved data found.");
                return new GameData();  // Если данных нет
            }

            var gameData = JsonConvert.DeserializeObject<GameData>(json);
            if (gameData == null)
            {
                Debug.LogWarning("Failed to deserialize game data.");
                return new GameData();  // Если данные повреждены
            }

            Debug.LogWarning("return gameData");
            return gameData;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game data: {e.Message}");
            return new GameData();  // Возвращаем пустые данные в случае ошибки
        }
    }

    // Сбрасываем данные в локальном хранилище
    public void Reset()
    {
        PlayerPrefs.DeleteKey(Key);  // Удаляем все сохраненные данные
        PlayerPrefs.Save();  // Применяем изменения
    }
}