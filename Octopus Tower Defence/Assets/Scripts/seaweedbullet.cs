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
    [SerializeField] private float despawn_time = 3f; // Time before despawning in seconds

    private Transform target;
    private float despawnTimer;
    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        despawnTimer = despawn_time;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            // Despawn if target is lost or despawn timer expires
            despawnTimer -= Time.deltaTime;
            if (despawnTimer <= 0 || Vector2.Distance(transform.position, initialPosition) > 10f)
            {
                Destroy(gameObject);
                Debug.Log("Bullet despawed UWU");
            }
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bullet_speed;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void OnCollisionEnter2D(Collision2D other)
{
    Enemy_Stats enemyStats = other.gameObject.GetComponent<Enemy_Stats>();
    if (enemyStats != null)
    {
        enemyStats.TakeDamage(bullet_damage);
    }

    Destroy(gameObject);
}

}
