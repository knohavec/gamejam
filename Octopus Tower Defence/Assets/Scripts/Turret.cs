using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turret_rotate_point;
    [SerializeField] private GameObject bullet_prefab;
    [SerializeField] private Transform firing_point;
    [SerializeField] private Tower tower; // Reference to the Tower instance

    [Header("Attributes")]

    [SerializeField] private LayerMask enemy_mask;
    [SerializeField] private float rotation_speed = 5f;
    [SerializeField] private float bps = 1f; //bullets per second
    
    

       private float targeting_range;
       private Transform target;

       private float time_until_fire;   

       
    private void OnDrawGizmosSelected()
    {
        if (transform == null)
    {
        Debug.LogError("Transform is null!");
        return;
    }

    // Assuming targetingRange is a field in your class
    if (targeting_range <= 0)
    {
        Debug.LogError("Invalid targeting range!");
        return;
    }

    // Draw the attack range in the scene view for debugging
    Gizmos.color = new Color(0, 1, 1, 0.5f); // Cyan color with 50% transparency
    Gizmos.DrawSphere(transform.position, targeting_range);
    }

 

    // Start is called before the first frame update
    void Start()
    {
        if (tower != null)
        {
            targeting_range = tower.tower_attack_range;
        }
        else
        {
            Debug.LogError("Tower reference is not set in Turret");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetIsInRange()){
            target = null;
        } else {
            time_until_fire += Time.deltaTime;

            if (time_until_fire >= 1f/bps){
                Shoot();
                time_until_fire = 0f;
            }

        }
    }




   private void Shoot()
{
    GameObject bulletobj = Instantiate(bullet_prefab, firing_point.position, Quaternion.identity);
    seaweedbullet bulletScript = bulletobj.GetComponent<seaweedbullet>();
    bulletScript.SetTarget(target);
}






    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targeting_range,
            (Vector2)transform.position, 0f, enemy_mask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) 
        * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turret_rotate_point.rotation = Quaternion.RotateTowards(turret_rotate_point.rotation, targetRotation,
        rotation_speed*Time.deltaTime);
    }

    private bool CheckTargetIsInRange(){
        return Vector2.Distance(target.position, transform.position) <= targeting_range;
    }
}
