using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Data;
using Interface;

namespace Services.SaveLoad
{
    public class CharacterHandler:ICharacterHandler
    {
        private readonly CharacterData _characterData;

        public CharacterHandler(CharacterData characterData)
        {
            _characterData = characterData;
        }

        public void SetSelectedCharacter(Character character)
        {
            _characterData.SetSelectedCharacter(character);
        }

        public Character GetSelectedCharacter() => _characterData.SelectedCharacter;
        
        public void SetActiveCharacters(List<Character> characters)
        {
            _characterData.ClearCharacters();
            foreach (var character in characters)
            {
                _characterData.AddActiveCharacter(character);
            }
        }
        
        public List<Character> GetActiveCharacters() => 
            _characterData.ActiveCharacters.ToList();

        public void SetInactiveHumanoids(List<Character> characters)
        {
            foreach (var character in characters)
            {
                _characterData.AddInactiveCharacter(character);
            }
        }

        public List<Character> GetInactiveHumanoids() =>
            _characterData.InactiveCharacters.ToList();
    }
}