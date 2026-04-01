using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Character Template")]
public class CharacterTemplateSO : ScriptableObject
{
    [Header("General parameters")] 
    [SerializeField] private string _name;
    [SerializeField] private string _description = "";
    [SerializeField] private int _lifeCount = 3;
    [SerializeField] private bool _playable;
    
    [Header("Jump parameters")]
    [SerializeField,Tooltip("Duration of jump in seconds")] private float _jumpDuration = 1f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private AnimationCurve _fallCurve;

    [Header("Slide parameters")] 
    [SerializeField] private float _slideDuration = 1f;
    
    [Header("Slide parameters")] 
    [SerializeField] private float _slideDownDuration = 1.5f;
    
    [Header("Collision Parameters")] 
    [SerializeField] private Vector3 _sphereCenter;
    [SerializeField] private float _sphereRadius;
    [SerializeField] private Vector3 _shrinkSphereCenter;
    [SerializeField] private float _shrinkSphereRadius;

    [Header("Components")]
    [SerializeField] private GameObject _model;
    
    public string Name => _name;
    public string Description => _description;
    public bool Playable => _playable;
    public GameObject Model => _model;
    public int LifeCount => _lifeCount;
    public float JumpDuration => _jumpDuration;
    public float JumpHeight => _jumpHeight;
    public AnimationCurve JumpCurve => _jumpCurve;
    public AnimationCurve FallCurve => _fallCurve;
    public float SlideDuration => _slideDuration;
    public float SlideDownDuration => _slideDownDuration;
    public Vector3 SphereCenter => _sphereCenter;
    public float SphereRadius => _sphereRadius;
    public Vector3 ShrinkSphereCenter => _shrinkSphereCenter;
    public float ShrinkSphereRadius => _shrinkSphereRadius;
}