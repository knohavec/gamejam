using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("Refrences")]
    [SerializeField] private GameObject[] tower_prefabs;
    
    private int selected_tower = 0;

    private void Awake(){
        main=this;
    }

    public GameObject GetSelectedTower(){
        return tower_prefabs[selected_tower];
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
