using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat_Manager : MonoBehaviour
{

   public GameObject enemyMeatPrefab;

    private void OnDestroy()
    {
        Instantiate(enemyMeatPrefab, transform.position, Quaternion.identity);
    }
}
