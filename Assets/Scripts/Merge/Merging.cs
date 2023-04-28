using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using UnityEngine;

namespace Merge
{
    public class Merging : MonoCache
    {
        [SerializeField] private Factory _factory;

        private Vector3 _tempPoint;
        
        public void Merge(Humanoid draggingHumanoid, Humanoid intoHumanoid)
        {
            if (draggingHumanoid.GetLevel() == intoHumanoid.GetLevel())
            {
                _tempPoint = intoHumanoid.gameObject.GetComponent<RectTransform>().position;
                int levelMerge = intoHumanoid.GetLevel();
                levelMerge++;

                draggingHumanoid.gameObject.SetActive(false);
                intoHumanoid.gameObject.SetActive(false);

                foreach (Humanoid humanoid in _factory.GetAllHumanoids)
                {
                    if (humanoid.GetLevel() == levelMerge 
                        && humanoid.gameObject.activeInHierarchy == false)
                    {
                        humanoid.gameObject.GetComponent<RectTransform>().position = _tempPoint;
                        humanoid.gameObject.SetActive(true);
                        return;
                    }
                }
            }

            if (draggingHumanoid.GetLevel() != intoHumanoid.GetLevel())
            {
                _tempPoint = draggingHumanoid.gameObject.GetComponent<RectTransform>().position;
                draggingHumanoid.InitPosition(intoHumanoid.ReadFirstPosition());
                intoHumanoid.InitPosition(_tempPoint);
            }
        }
    }
}