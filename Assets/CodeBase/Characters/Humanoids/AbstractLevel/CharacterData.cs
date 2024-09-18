using UnityEngine;

namespace Characters.Humanoids.AbstractLevel
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public CharacterType Type;
        public int Health;
        public int Price;
        public SpriteRenderer Ring;
        public Sprite Sprite;
        public bool CanMove;
    }
}