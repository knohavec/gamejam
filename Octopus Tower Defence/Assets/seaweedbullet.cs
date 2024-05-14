using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seaweedbullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Bullet Stats")]
    [SerializeField] private float bullet_speed = 4f;
    [SerializeField] private int bullet_damage = 1;

    private Transform target;

    private void FixedUpdate(){
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bullet_speed;
    }

    public void SetTarget(Transform _target){
        target = _target;
}

    private void OnCollisionEnter2D(Collision2D other){
        other.gameObject.GetComponent<Enemy_Stats>().TakeDamage(bullet_damage);
        Destroy(gameObject);
        // Debug.Log("HIT");
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
