using Components.SaveService;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int _lifeCount = 3;
    
    private CharacterTemplateSO _template;

    private int _currentLifeCount;
    
    private void Start()
    {
        var save = SaveService.Load();
        if (save != null && !string.IsNullOrEmpty(save.SelectedCharacterName))
        {
            _template = ScriptableObjectDataBase.Get<CharacterTemplateSO>(save.SelectedCharacterName);
            _currentLifeCount = _template.LifeCount;
        }
        else
        {
            Debug.LogError("No character selected");
            _currentLifeCount = _lifeCount;
        }
        
        EventSystem.OnPlayerLifeUpdated?.Invoke(_currentLifeCount);
        EventSystem.OnPlayerCollision += HandlePlayerCollision;
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerCollision -= HandlePlayerCollision;
    }

    private void HandlePlayerCollision()
    {
        if (_currentLifeCount - 1 < 0)
        {
            // The player is dead
            return;
        }
        
        _currentLifeCount--;
        EventSystem.OnPlayerLifeUpdated?.Invoke(_currentLifeCount);
    }
}
