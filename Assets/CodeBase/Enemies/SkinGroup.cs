using UnityEngine;

namespace Enemies
{
    public class SkinGroup : MonoBehaviour
    {
        public bool IsSpecial;
        public void Initialize(int index)
        {
            if (IsSpecial)
            {
                transform.GetChild(index).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
            }
        }
    }
}