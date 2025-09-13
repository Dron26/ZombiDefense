using Data;
using Infrastructure.AssetManagement;
using UnityEngine;

namespace UI.Locations
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Locations/LocationData")]
    public class LocationData : ScriptableObject
    {
        [Header("Base Settings")]
        public int Id;
        public bool IsTutorial;
        public bool IsLocked;
        public bool IsCompleted;

        [Header("Wave Settings")]
        //public int BaseZombieHealth;
        public int BaseReward;
        
        public WavesContainerData WavesContainerData;
        public bool IsAdditional;
        public int UnlockedId;
        
        [Header("Mission Description")]
        [Tooltip("Название миссии, отображается в UI")]
        public string TitleRu;
        public string TitleEn;
        public string TitleTr;
        [Tooltip("Краткий контекст миссии (1-2 предложения)")]
        [TextArea(2, 4)]
        public string ContextRu;
        public string ContextEn;
        public string ContextTr;
        [Tooltip("Цель миссии (зачистка, удержание и т.д.)")]
        [TextArea(2, 4)]
        public string ObjectiveRu;
        public string ObjectiveEn;
        public string ObjectiveTr;
        [Tooltip("Описание локации (где происходит миссия)")]
        public string LocationRu;
        public string LocationEn;
        public string LocationTr;
        [Tooltip("Подсказка для игрока (рекомендации по юнитам/механикам)")]
        [TextArea(2, 4)]
        public string TipRu;
        public string TipEn;
        public string TipTr;
    }
}