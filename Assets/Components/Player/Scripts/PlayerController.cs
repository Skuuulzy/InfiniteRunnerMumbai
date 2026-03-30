using Components.SaveService;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _movementController;
    [SerializeField] private PlayerCollisionController _collisionController;
    [SerializeField] private Transform _modelHolder;
    
    [Header("Debug")]
    [SerializeField] private bool _useDebugTemplate;
    [SerializeField] private CharacterTemplateSO _template;
    
    private void Awake()
    {
        if (_useDebugTemplate && _template)
        {
            Initialize(_template);
            return;
        }
        
        var save = SaveService.Load();
        if (save != null && !string.IsNullOrEmpty(save.SelectedCharacterName))
        {
            _template = ScriptableObjectDataBase.Get<CharacterTemplateSO>(save.SelectedCharacterName);
            Initialize(_template);
        }
    }

    private void Initialize(CharacterTemplateSO template)
    {
        var model = Instantiate(template.Model, _modelHolder);
        var animator = model.GetComponent<Animator>();
        
        _movementController.Initialize(template, animator);
        _collisionController.Initialize(template);
    }
}
