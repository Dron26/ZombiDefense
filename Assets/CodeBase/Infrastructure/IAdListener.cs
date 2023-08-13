using Service.Ads;

namespace Infrastructure
{
    public interface IAdListener
    {
        void Construct( IAdsService adsService);
    }
}