using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;


    [Header("Spawn Settings")]
    [SerializeField] private int base_enemies = 8;
    [SerializeField] private float enemies_per_sec = 0.5f;
    [SerializeField] private float time_between_waves = 5f;
    [SerializeField] private float difficulty_scaling = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();


    private int current_wave = 1;
    private float time_since_last_spawn;
    private int enemies_alive;
    private int enemies_left_to_spawn;
    private bool isSpawning = false;


    private void Awake(){
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start(){       
        StartCoroutine( StartWave());
    }
    private void Update(){
        if (!isSpawning) return;

        time_since_last_spawn += Time.deltaTime;

        if (time_since_last_spawn >= (1f / enemies_per_sec) && enemies_left_to_spawn > 0){
            
            SpawnEnemy();
            enemies_left_to_spawn--;
            enemies_alive ++;
            time_since_last_spawn = 0f;
        }

        if (enemies_alive==0 && enemies_left_to_spawn == 0){
            EndWave();
        }

    }
    private void EndWave(){
        isSpawning=false;
        time_since_last_spawn=0f;
        current_wave++;
        StartCoroutine(StartWave());
    }
    private void EnemyDestroyed(){
        enemies_alive--;
    }
    private IEnumerator StartWave(){
        yield return new WaitForSeconds(time_between_waves);
        isSpawning=true;
        enemies_left_to_spawn = EnemiesPerWave();
        
    }
    private int EnemiesPerWave(){
    int enemies = Mathf.RoundToInt(base_enemies * Mathf.Pow(current_wave, difficulty_scaling));
    return Mathf.Max(enemies, 1);
    }

    private void SpawnEnemy(){
        GameObject prefab_to_spawn = enemyPrefabs[0];
        Instantiate(prefab_to_spawn, LevelManager.main.startPoint.position,Quaternion.identity);
    }
  



}
