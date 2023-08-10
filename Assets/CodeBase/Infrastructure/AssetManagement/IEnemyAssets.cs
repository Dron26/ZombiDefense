using Enemies.AbstractEntity;

namespace Infrastructure.AssetManagement
{
    public interface  IEnemyAssets
    {
        Enemy LoadEnemy(string path);
    }
}