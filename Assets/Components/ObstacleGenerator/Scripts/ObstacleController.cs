using System;
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
    
    [Header("Components")]
    [SerializeField] private ChunkController[] _chunksPool;
    
    private readonly List<ChunkController> _instancedChunks = new();
    private float _baseTranslationSpeed;
    
    private float _stopDelayTimer;
    private bool _stopped;

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
        ResetMovementAfterDelay();
        
        foreach (var chunk in _instancedChunks)
        {
            chunk.transform.Translate(Vector3.back * _translationSpeed * Time.deltaTime);
        }

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
            var chunk = AddChunk(LastActiveChunk().EndAnchor);
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

            var chunk = AddChunk(LastActiveChunk().EndAnchor);
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
        if (newState is not GameState)
        {
            EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
            return;
        }

        _translationSpeed = _baseTranslationSpeed;
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
    }
}