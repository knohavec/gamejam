using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private int hit_points = 2;
    [SerializeField] private int damage = 2;

    private bool is_destroyed = false;

    public void TakeDamage(int dmg){
        hit_points -= dmg;

        if (hit_points <=0 && !is_destroyed){
            EnemySpawner.onEnemyDestroy.Invoke();
            is_destroyed = true;
            Destroy(gameObject);
        }

    }


    




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
