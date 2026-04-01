using System.Collections.Generic;
using Components.SaveService;
using TMPro;
using UnityEngine;

public class CharacterSelectorController : MonoBehaviour
{
    [SerializeField] private Transform _characterHolder;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _characterJumpHeightText;
    [SerializeField] private TMP_Text _characterJumpTimeText;
    
    private List<CharacterTemplateSO> _characters;
    private int _currentCharacterIndex;
    
    private void Awake()
    {
        _characters = ScriptableObjectDataBase.GetAll<CharacterTemplateSO>();
        if (_characters.Count == 0)
        {
            Debug.LogError("No characters found, make sure you have CharacterTemplateSO inside resources folder.");
            return;
        }
        
        SetUpCharacter(0);
    }

    private void SetUpCharacter(int index)
    {
        if (index >= _characters.Count)
        {
            Debug.LogError("Character index out of range");
            return;
        }
        
        _currentCharacterIndex = index;
        CharacterTemplateSO characterTemplate = _characters[index];
        
        // Delete potentially existing character
        foreach (Transform child in _characterHolder)
        {
            Destroy(child.gameObject);
        }
        
        // Instantiate new character model
        Instantiate(characterTemplate.Model, _characterHolder);
        
        // Setup UI Data
        _characterNameText.text = characterTemplate.Name;
        _characterJumpHeightText.text = $"Jump Height: {characterTemplate.JumpHeight}m";
        _characterJumpTimeText.text = $"Jump Duration: {characterTemplate.JumpDuration}s";
    }
    
    public void NextCharacter()
    {
        var nextIndex = _currentCharacterIndex + 1;
        
        if (nextIndex >= _characters.Count)
            nextIndex = 0;
        
        SetUpCharacter(nextIndex);
    }

    public void PreviousCharacter()
    {
        var previousIndex = _currentCharacterIndex - 1;
        
        if (previousIndex == 0)
            previousIndex = _characters.Count - 1;
        
        SetUpCharacter(previousIndex);
    }
    
    public void PlayCharacter()
    {
        var saveData = SaveService.Load();
        
        saveData.RunCount++;
        saveData.SelectedCharacterName = _characters[_currentCharacterIndex].name;
        SaveService.Save(saveData);
        
        SceneLoaderService.LoadGame();
    }
}