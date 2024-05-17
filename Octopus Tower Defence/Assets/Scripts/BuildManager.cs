using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;
    
    [Header("References")]
    [SerializeField] private Tower[] towers;
    [SerializeField] private int selectedTower = -1;
    
    

   

    private void Awake()
    {
        main = this;
        
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void SelectTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }

    // Start is called before the first frame update

    public void ClearSelectedTower(){
        selectedTower = -1;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
