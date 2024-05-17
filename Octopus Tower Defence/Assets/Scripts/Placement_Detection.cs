using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement_Detection : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Color hover_color;
    [SerializeField] private float yOffset;

    private GameObject tower;
    private Color start_color;


    // Start is called before the first frame update
    private void Start()
    {
        start_color = sr.color;
    }

    private void OnMouseEnter()
    
    {
        // Debug.Log("Mouse Detected");
        sr.color = hover_color;
    }

     private void OnMouseExit()

    {
        sr.color = start_color;
    }

   private void OnMouseDown()
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("Tower"))
        {
            Debug.Log("Tower already present!");
            return;
        }
    }

    if (tower != null) return;

    Tower tower_to_build = BuildManager.main.GetSelectedTower();

    if (tower_to_build.towercost > SandDollarSpawning.Instance.SandDollarTotal)
    {
        Debug.Log("Too Expensive");
        return;
    }

    SandDollarSpawning.SpendSandDollars(tower_to_build.towercost);

    Instantiate(tower_to_build.towerprefab, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), Quaternion.identity);
}




    // Update is called once per frame
    void Update()
    {
        
    }
}
