using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Tower : MonoBehaviour
{
    public string towername;
    public int towercost;
    public GameObject towerprefab;
 
    public int towerhealth;

    public bool isDestroyed = false;

    public float tower_attack_range;

    public int towerdamage;

    public float tower_attack_speed;

    public Tower (string _name, float _attackspeed, int _damage, int _cost, GameObject _prefab, int _health, float _attackrange){
        towername = _name;
        towercost = _cost;
        towerprefab = _prefab;
        tower_attack_range = _attackrange;
        towerhealth = _health;
        towerdamage = _damage;
        tower_attack_speed = _attackspeed;
    }

    public void TakeDamage(int dmg)
    {
        towerhealth -= dmg;

        if (towerhealth <= 0)
        {
            Destroy(gameObject);
            isDestroyed = true;
        }
    }

    // You can add other methods or properties as needed
}
