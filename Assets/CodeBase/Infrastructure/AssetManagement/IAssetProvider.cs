using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject Instantiate(string path);
    }
}