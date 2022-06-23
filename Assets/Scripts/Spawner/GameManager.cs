using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    [SerializeField] UnityEvent m_UpgradeEvent = new UnityEvent();
    [SerializeField] UnityEvent m_PauseEvent = new UnityEvent();
    [SerializeField] UnityEvent m_UnPauseEvent = new UnityEvent();
    [SerializeField] int _enemiesToSpawn;
    [SerializeField] float _enemiesSpawnRate = 5;
    [SerializeField] int _enemyToSpawnIncrease = 20;
    [Tooltip("Drag and Drop the enemies you want to be spawned")] [SerializeField] Character[] _enemiesData;
    [SerializeField] private bool _SpawnNow = false;
    [SerializeField] private float _everyXsecIncreaseDifficulty = 120;
    public bool spawnEnemies = true;
    
    private float _timer = 0;
    private float _spawntimer = 0;
    private string _enemyNameToSpawn;
    private int _waveCount = 0;
    private int _counterCurrentEnemyNameIndex = 0;

    private void Awake()
    {
        _waveCount = 0;
        _enemyNameToSpawn = _enemiesData[0].charName;
        Time.timeScale = 1;

    }

    void Update()
    {
        _spawntimer += Time.deltaTime;
        if (spawnEnemies)
        {
            if (_spawntimer > _enemiesSpawnRate || _SpawnNow)
                {
                    EnemiesSpawner.m_enemiesSpawnerInstance.SpawnEnemies(_enemiesToSpawn, _enemyNameToSpawn);
                    _spawntimer = 0;
                    _waveCount++;
                    _SpawnNow = false;
                }
        }
        _timer += Time.deltaTime;
        if (_timer > _everyXsecIncreaseDifficulty)
        {
            m_UpgradeEvent.Invoke();
            PauseUnpause.PauseUnpauseInstance.Pause();
            _timer = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && PauseUnpause.PauseUnpauseInstance.IsPause())
        {
            Debug.Log("Trying to UNpause");

            UnPause();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !PauseUnpause.PauseUnpauseInstance.IsPause())
        {
            Debug.Log("Trying to pause");
            Pause();
        }   
           

    }

    public void IncreaseEnemiesDifficulty()
    {
        Debug.Log("Increasing difficulty!");
        Debug.Log(_enemiesData.Length);
        _enemyNameToSpawn = _enemiesData[_counterCurrentEnemyNameIndex].charName;
        _enemiesToSpawn += _enemyToSpawnIncrease;
        if (_counterCurrentEnemyNameIndex < _enemiesData.Length-1) _counterCurrentEnemyNameIndex++;
    }

    public void UnPause()
    {
        m_UnPauseEvent.Invoke();
        PauseUnpause.PauseUnpauseInstance.UnPause();

    }
    public void Pause()
    {
        m_PauseEvent.Invoke();
        PauseUnpause.PauseUnpauseInstance.Pause();

    }
}
