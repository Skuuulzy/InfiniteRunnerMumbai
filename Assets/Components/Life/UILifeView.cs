using TMPro;
using UnityEngine;

public class UILifeView : MonoBehaviour
{
    [SerializeField] private TMP_Text _lifeText;

    private void Awake()
    {
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
    }

    private void HandlePlayerLifeUpdated(int newLifeCount)
    {
        _lifeText.text = "Lives: " + newLifeCount;
    }
}