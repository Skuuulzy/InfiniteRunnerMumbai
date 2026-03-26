using Components.SaveService;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterTemplateSO _template;
    
    private void Awake()
    {
        var save = SaveService.Load();
        if (save != null && !string.IsNullOrEmpty(save.SelectedCharacterName))
        {
            _template = ScriptableObjectDataBase.Get<CharacterTemplateSO>(save.SelectedCharacterName);
            Initialize(_template);
        }
    }

    private void Initialize(CharacterTemplateSO template)
    {
        var model = Instantiate(template.Model, transform);
    }
}
