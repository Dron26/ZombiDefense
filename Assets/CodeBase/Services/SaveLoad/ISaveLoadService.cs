using Data;

namespace Services.SaveLoad
{
    public interface ISaveLoadService: IService
    {
        void SaveData();
        GameData LoadData();
    }
}