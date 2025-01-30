using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;

namespace Data
{
    public class CharacterData
    {
        private Character _selectedCharacter;
        private readonly List<Character> _activeCharacters = new();
        private readonly List<Character> _inactiveCharacters = new();
        private readonly List<Character> _availableCharacters = new();
        
        public IReadOnlyList<Character> ActiveCharacters => _activeCharacters.AsReadOnly();
        public IReadOnlyList<Character> InactiveCharacters => _inactiveCharacters.AsReadOnly();
        public IReadOnlyList<Character> AvailableCharacters => _availableCharacters.AsReadOnly();

        public Character SelectedCharacter => _selectedCharacter;

        public void SetSelectedCharacter(Character character) => _selectedCharacter = character;

        public void AddActiveCharacter(Character character) => _activeCharacters.Add(character);
        public void AddInactiveCharacter(Character character) => _inactiveCharacters.Add(character);
        public void AddAvailableCharacter(Character character) => _availableCharacters.Add(character);

        public void ClearActiveCharacters()
        {
            _activeCharacters.Clear();
        }
        public void ClearInactiveCharacters()
        {
            _inactiveCharacters.Clear();
        }
        public void ClearAvailableCharacters()
        {
            _availableCharacters.Clear();
        }

        public void Reset()
        {
            ClearActiveCharacters();
            ClearInactiveCharacters();
            ClearAvailableCharacters();
        }
    }
}