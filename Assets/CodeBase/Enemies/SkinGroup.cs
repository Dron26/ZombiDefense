using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinGroup : MonoBehaviour
{
   [SerializeField] private List <SkinnedMeshRenderer> meshes = new List<SkinnedMeshRenderer>();

   public void Initialize()
   {
      
   }

   public int GetCountMeshes()
   {
      return meshes.Count;
   } public void SetMesh(int number)
   {
      meshes[number].gameObject.SetActive(true);
   }
}
