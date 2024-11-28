using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
   public class SkinGroup : MonoBehaviour
   {
      public void Initialize()
      {
         transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
      }
   }
}
