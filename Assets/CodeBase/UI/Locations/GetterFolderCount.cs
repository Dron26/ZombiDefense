using System.IO;
using Infrastructure.AssetManagement;

namespace UI.Locations
{
    public static class GetterFolderCount
    {
        public static int GetFolderItemsCount(string path)
        {
            var directoryInfo = new DirectoryInfo($"{AssetPaths.Resources}{path}");

            int objectsCount = directoryInfo.GetFiles(
                    "*.asset", 
                    SearchOption.TopDirectoryOnly)
                .Length;

            return objectsCount;
        }
    }
}