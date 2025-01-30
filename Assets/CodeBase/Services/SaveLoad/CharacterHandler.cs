using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Interface;
using CharacterData = Data.CharacterData;

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
        
        public void SetActiveCharacters(List<Character> characters)
        {
            _characterData.ClearActiveCharacters();
            
            foreach (var character in characters) _characterData.AddActiveCharacter(character);
        }
        public Character GetSelectedCharacter() => _characterData.SelectedCharacter;
        
        public List<Character> GetAvailableCharacter() => _characterData.AvailableCharacters.ToList();
        public List<Character> GetActiveCharacters() => _characterData.ActiveCharacters.ToList();

        public List<Character> GetInactiveHumanoids() => _characterData.InactiveCharacters.ToList();
        public void SetInactiveHumanoids(List<Character> characters)
        {
            foreach (var character in characters)
            {
                _characterData.AddInactiveCharacter(character);
            }
        }


        public void Reset()
        {
            _characterData.Reset();
        }
    }
}