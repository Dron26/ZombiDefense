using Newtonsoft.Json;

namespace Services.SaveLoad
{
    public class JsonSerializationService : ISerializationService
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings 
        { 
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
        };

        public string Serialize<T>(T data) => JsonConvert.SerializeObject(data, Formatting.Indented, _settings);

        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}