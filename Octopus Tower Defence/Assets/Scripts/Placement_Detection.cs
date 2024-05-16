using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement_Detection : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Color hover_color;

    private GameObject tower;
    private Color start_color;


    // Start is called before the first frame update
    private void Start()
    {
        start_color = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hover_color;
    }

     private void OnMouseExit()
    {
        sr.color = start_color;
    }

   private void OnMouseDown()
{
    if(tower == null) return;

    Tower tower_to_build = BuildManager.main.GetSelectedTower();

    if (tower_to_build.cost > SandDollarSpawning.Instance.SandDollarTotal){
        Debug.Log("Too Expensive");
        return;
    }

    SandDollarSpawning.SpendSandDollars(tower_to_build.cost);

    Instantiate(tower_to_build.prefab, transform.position, Quaternion.identity);
}



    // Update is called once per frame
    void Update()
    {
        
    }
}
