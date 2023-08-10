using Service;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssetsProvider: IService
    {
        GameObject Instantiate(string path);
    }
}