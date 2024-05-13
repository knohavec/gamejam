using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;


    [Header("Spawn Settings")]
    [SerializeField] private int base_enemies = 8;
    [SerializeField] private float enemies_per_sec = 0.5f;
    [SerializeField] private float time_between_waves = 5f;
    [SerializeField] private float difficulty_scaling = 0.75f;

    private int current_wave = 1;
    private float time_since_last_spawn;
    private int enemies_alive;
    private int enemies_remaining;
    private bool isSpawning = false;


    private void Start(){
        StartWave();
    }
    private void Update(){
        if (!isSpawning) return;

        time_since_last_spawn += Time.deltaTime;

        if (time_since_last_spawn >= (1f / enemies_per_sec) && enemies_remaining > 0){
            
            SpawnEnemy();
            enemies_remaining--;
            enemies_alive ++;
            time_since_last_spawn = 0f;
        }
    }
    private void StartWave(){
        isSpawning=true;
        enemies_remaining = EnemiesPerWave();
    }
    private int EnemiesPerWave(){
        return Mathf.RoundToInt(base_enemies*Mathf.Pow(current_wave, difficulty_scaling));
    }
    private void SpawnEnemy(){
        GameObject prefab_to_spawn = enemyPrefabs[0];
        Instantiate(prefab_to_spawn, LevelManager.main.startPoint.position,Quaternion.identity);
    }
  



}
