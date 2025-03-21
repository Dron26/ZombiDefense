using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Data;
using Interface;

namespace Services.SaveLoad
{
    public class CharacterHandler:ICharacterHandler
    {
        private readonly CharactersData _charactersData;
        private IGameEventBroadcaster _eventBroadcaster;

        public CharacterHandler(CharactersData charactersData)
        {
            _charactersData = charactersData;
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
        }

        public void SetSelectedCharacter(Character character)
        {
            _charactersData.SetSelectedCharacter(character);
            if (character.TryGetComponent(out Humanoid humanoid))
            {
                _eventBroadcaster.InvokeOnSelectedHumanoid(humanoid);
            }
        }
        
        public void SetActiveCharacters(List<Character> characters)
        {
            _charactersData.ClearActiveCharacters();
            
            foreach (var character in characters) _charactersData.AddActiveCharacter(character);
        }
        public void SetActiveCharacter(Character character)
        {
            _charactersData.AddActiveCharacter(character);
        }

        public void RemoveCharacter(Character character)
        {
            _charactersData.RemoveCharacter(character);
        }

        
        public Character GetSelectedCharacter() => _charactersData.SelectedCharacter;
        
        public List<Character> GetAvailableCharacter() => _charactersData.AvailableCharacters.ToList();
        public List<Character> GetActiveCharacters() => _charactersData.ActiveCharacters.ToList();

        private void AddListener()
        {
            _eventBroadcaster.OnSetActiveCharacter += SetActiveCharacter;
            _eventBroadcaster.OnCharacterDie += RemoveCharacter;
        }

        private void RemoveListener()
        {
            _eventBroadcaster.OnSetActiveCharacter -= SetActiveCharacter;
            _eventBroadcaster.OnCharacterDie -= RemoveCharacter;
        }


        public void Reset()
        {
            _charactersData.Reset();
            RemoveListener();
        }
    }
}