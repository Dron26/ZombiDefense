public class SceneObjectManager : MonoBehaviour
{
    private BoxFactory _boxFactory;
    private CharacterFactory _characterFactory;
    
    public void Initialize(BoxFactory boxFactory, CharacterFactory characterFactory)
    {
        _boxFactory = boxFactory;
        _characterFactory = characterFactory;

        // Подписка на события магазина
        Store.OnItemSelected += HandleItemSelected;
        Store.OnCharacterSelected += HandleCharacterSelected;
    }

    private void HandleItemSelected(BoxData boxData)
    {
        // Создание бокса через фабрику
        var box = _boxFactory.Create(boxData);
        PlaceBoxOnScene(box);
    }

    private void HandleCharacterSelected(CharacterData characterData)
    {
        // Создание персонажа через фабрику
        var character = _characterFactory.Create(characterData);
        PlaceCharacterOnScene(character);
    }

    private void PlaceBoxOnScene(Box box)
    {
        // Логика размещения бокса на сцене
    }

    private void PlaceCharacterOnScene(Character character)
    {
        // Логика размещения персонажа на сцене
    }

    private void OnDestroy()
    {
        // Отписка от событий при уничтожении системы
        Store.OnItemSelected -= HandleItemSelected;
        Store.OnCharacterSelected -= HandleCharacterSelected;
    }
}