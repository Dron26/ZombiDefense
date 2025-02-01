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
        if (PlayerPrefs.HasKey(Key))  // Проверяем, есть ли сохраненные данные
            return JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(Key));  // Загружаем данные
        return new GameData();  // Возвращаем новые данные, если сохранений нет
    }

    // Сбрасываем данные в локальном хранилище
    public void Reset()
    {
        PlayerPrefs.DeleteKey(Key);  // Удаляем все сохраненные данные
        PlayerPrefs.Save();  // Применяем изменения
    }
}