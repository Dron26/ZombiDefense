using Data;
using Interface;
using Newtonsoft.Json;

namespace Services.SaveLoad
{
    public class CloudDataPersistence : IDataPersistence
    {
        public void Save(GameData data)
        {
            string json = JsonConvert.SerializeObject(data);
            // Тут будет код для сохранения в облаке
            // Например, PlayerAccount.SetCloudSaveData(json);
        }

        // Загружаем данные из облака
        public GameData Load()
        {
            // Здесь код для загрузки данных из облака
            // Например, string json = PlayerAccount.GetCloudSaveData();
            // return !string.IsNullOrEmpty(json) ? JsonConvert.DeserializeObject<GameData>(json) : new GameData();
            return new GameData();  // Пока возвращаем пустые данные
        }

        // Сбрасываем данные в облаке
        public void Reset()
        {
            // Сбрасываем данные в облаке
            // Например, PlayerAccount.SetCloudSaveData("{}");
        }
    }
}