using YG;

namespace Integration
{
    public class PaymentManager
    {
        private void OnEnable()
        {
            YG2.onPurchaseSuccess += SuccessPurchased;       
        }

        private void OnDisable()
        {
            YG2.onPurchaseSuccess -= SuccessPurchased;
        }

        private void SuccessPurchased(string id)
        {
            YG2.SetState(id, 1);
        }
        
    }
}