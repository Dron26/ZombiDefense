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
        List<Character> GetActiveCharacters();
        void SetInactiveHumanoids(List<Character> characters);
        List<Character> GetInactiveHumanoids();
        void Reset();
        List<Character> GetAvailableCharacter();
    }
}