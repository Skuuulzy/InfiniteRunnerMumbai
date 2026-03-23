using Components.SaveService;
using UnityEngine;
using UnityEngine.UI;

public class StatsModifierController : MonoBehaviour
{
    [SerializeField] private Slider _slideSpeedSlider;

    private void Awake()
    {
        var saveData = SaveService.Load();
        _slideSpeedSlider.SetValueWithoutNotify(saveData.SlideSpeed);
    }

    public void UpdateSlideSpeed(float value)
    {
        var saveData = SaveService.Load();
        
        saveData.SlideSpeed = value;
        SaveService.Save(saveData);
    }
}