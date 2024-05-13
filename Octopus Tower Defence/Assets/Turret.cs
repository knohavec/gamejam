using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turret_rotate_point;


    [Header("Attribute")]
    [SerializeField] private float targeting_range = 5f;
    [SerializeField] private LayerMask enemy_mask;
    [SerializeField] private float rotation_speed = 5f;

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targeting_range);
    }

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {

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
        }
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
