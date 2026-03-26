using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Character Template")]
public class CharacterTemplateSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _model;

    [Header("General parameters")] [SerializeField]
    private int _lifeCount = 3;

    public string Name => _name;
    public GameObject Model => _model;
    public int LifeCount => _lifeCount;
}