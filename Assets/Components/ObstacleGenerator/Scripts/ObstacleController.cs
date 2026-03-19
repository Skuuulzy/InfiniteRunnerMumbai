using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField, Tooltip("Translation speed of chunks in m/s")] private float _translationSpeed = 1f;
    [SerializeField] private int _activeChunkCount = 5;
    [SerializeField] private int _behindChunkCount = 1;
    [SerializeField] private float _stopDelayOnDamage = 0.2f;
    [SerializeField] private float _distanceToNextChunk = 5f;
    
    [Header("Components")]
    [SerializeField] private ChunkController[] _chunksPool;

    [Header("Speed Up")] 
    [SerializeField, Tooltip("Interval in seconds between each speed increases")] private float _speedUpInterval = 15f;
    [SerializeField, Tooltip("Speed increase applied on each interval")] private float _speedUpIncrease = 1.5f;
    
    private readonly List<ChunkController> _instancedChunks = new();
    private float _baseTranslationSpeed;
    
    private float _stopDelayTimer;
    private bool _stopped;
    private bool _inGameState;

    private GameState _gameState;
    private int _lastSpeedUpTime; // Last time when speed up was applied to avoid multiple speed ups on same second interval.
    
    private void Awake()
    {
        EventSystem.OnStateChanged += HandleStateChanged;
    }

    private void Start()
    {
        _baseTranslationSpeed = _translationSpeed;
        _translationSpeed = 0;
        
        AddBaseChunk();
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
        EventSystem.OnStateChanged -= HandleStateChanged;
    }

    private void Update()
    {
        if (!_inGameState)
        {
            return;
        }
        
        ResetMovementAfterDelay();
        TranslateChunks();
        UpdateChunks();
    }

    private void ResetMovementAfterDelay()
    {
        if (!_stopped) 
            return;
        
        _stopDelayTimer += Time.deltaTime;
        if (_stopDelayTimer >= _stopDelayOnDamage)
        {
            _stopped = false;
            _translationSpeed = _baseTranslationSpeed;
            _stopDelayTimer = 0f;
        }
    }
    
    private void TranslateChunks()
    {
        var gameTimer = _gameState.Timer;
        if (gameTimer != 0 && gameTimer % _speedUpInterval == 0 && gameTimer != _lastSpeedUpTime)
        {
            _translationSpeed += _speedUpIncrease;
            _baseTranslationSpeed = _translationSpeed;
            _lastSpeedUpTime = gameTimer;
        }
        
        foreach (var chunk in _instancedChunks)
        {
            chunk.transform.Translate(Vector3.back * (_translationSpeed * Time.deltaTime));
        }
    }

    private void UpdateChunks()
    {
        List<ChunkController> behindChunks = new();

        foreach (var chunk in _instancedChunks)
        {
            if (chunk.IsBehindPlayer())
            {
                behindChunks.Add(chunk);
            }
        }

        // Delete potential chunks behind player.
        if (behindChunks.Count > _behindChunkCount)
        {
            int chunkToDeleteCount = behindChunks.Count - _behindChunkCount;

            for (int i = 0; i < chunkToDeleteCount; i++)
            {
                var chunkToDelete = behindChunks[i];
                _instancedChunks.Remove(chunkToDelete);
                
                Destroy(chunkToDelete.gameObject);
            }
        }
        
        // Add potential new chunks.
        int missingChunkCount = _activeChunkCount - _instancedChunks.Count;
        for (int i = 0; i < missingChunkCount; i++)
        {
            var chunk = AddChunk(LastActiveChunk().EndAnchor + new Vector3(0, 0, _distanceToNextChunk));
            _instancedChunks.Add(chunk);
        }
    }

    private void AddBaseChunk()
    {
        for (int i = 0; i < _activeChunkCount; i++)
        {
            if (i == 0)
            {
                var baseChunk = AddChunk(transform.position);
                _instancedChunks.Add(baseChunk);
                continue;
            }

            var chunk = AddChunk(LastActiveChunk().EndAnchor + new Vector3(0, 0, _distanceToNextChunk));
            _instancedChunks.Add(chunk);
        }
    }

    private ChunkController AddChunk(Vector3 position)
    {
        if (_chunksPool.Length == 0)
        {
            Debug.LogError("No chunks in pool");
            return null;
        }
        
        var index = Random.Range(0, _chunksPool.Length);
        ChunkController chunk = Instantiate(_chunksPool[index], position, Quaternion.identity);
        
        return chunk;
    }
    
    private ChunkController LastActiveChunk()
    {
        return _instancedChunks[_instancedChunks.Count - 1];
    }
    
    private void HandlePlayerLifeUpdated(int playerLifeCount)
    {
        if (playerLifeCount > 0)
        {
            _stopped = true;
        }
        
        _translationSpeed = 0;
    }
    
    private void HandleStateChanged(State newState)
    {
        if (newState is not GameState gameState)
        {
            EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
            _inGameState = false;
            return;
        }

        _gameState = gameState;
        _translationSpeed = _baseTranslationSpeed;
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
        _inGameState = true;
    }
}