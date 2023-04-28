using Service;
using UnityEngine;

namespace Infrastructure.AssetManagement
{
    public interface IAssets:IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path,Transform transform);
        
    }
}