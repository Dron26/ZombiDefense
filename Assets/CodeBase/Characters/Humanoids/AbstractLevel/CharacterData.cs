using Infrastructure.AIBattle.AdditionalEquipment;
using UnityEngine;

namespace Characters.Humanoids.AbstractLevel
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public CharacterType Type;
        public ItemData ItemData;
        public int Health;
        public int Price;
        public SpriteRenderer Ring;
        public Sprite Sprite;
        public bool CanMove;
        
    }
}