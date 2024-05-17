using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement_Detection : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Color hover_color;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;

    private GameObject tower;
    private Color start_color;
    private bool hasTower = false; // Added boolean variable to track tower presence

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
        if (hasTower) // Check if a tower is already present
        {
            Debug.Log("Tower already present!");
            return;
        }

        Tower tower_to_build = BuildManager.main.GetSelectedTower();
        BuildManager.main.ClearSelectedTower();

        if (tower_to_build.towercost > SandDollarSpawning.Instance.SandDollarTotal)
        {
            Debug.Log("Too Expensive");
            return;
        }

        SandDollarSpawning.SpendSandDollars(tower_to_build.towercost);

        Instantiate(tower_to_build.towerprefab, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z + zOffset), Quaternion.identity);

        hasTower = true; // Set to true after placing tower
    }

    // Update is called once per frame
    void Update()
    {

    }
}
