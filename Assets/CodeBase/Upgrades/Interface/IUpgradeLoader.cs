using System.Collections.Generic;

namespace Services
{
    public interface IUpgradeLoader
    {
        public List<UpgradeData> GetData();
    }
}