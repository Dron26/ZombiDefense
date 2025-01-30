namespace Services.SaveLoad
{
    public interface ISerializationService
    {
        string Serialize<T>(T data);
        T Deserialize<T>(string json);
    }
}