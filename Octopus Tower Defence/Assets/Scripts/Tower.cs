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

    public Tower (string _name, int _cost, GameObject _prefab){
        towername = _name;
        towercost = _cost;
        towerprefab = _prefab;
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
