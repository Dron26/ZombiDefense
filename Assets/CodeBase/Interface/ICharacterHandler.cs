using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Services;

namespace Interface
{
    public interface ICharacterHandler:IService
    {
        void SetSelectedCharacter(Character character);
        Character GetSelectedCharacter();
        void SetActiveCharacters(List<Character> characters);
        void SetActiveCharacter(Character character);
        List<Character> GetActiveCharacters();
        void Reset();
        List<Character> GetAvailableCharacter();
    }
}