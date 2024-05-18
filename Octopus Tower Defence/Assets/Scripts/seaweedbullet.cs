using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seaweedbullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Bullet Stats")]
    [SerializeField] private float bullet_speed = 4f;
    [SerializeField] private float bullet_damage = 1f;
    [SerializeField] private float despawn_time = 3f; // Time before despawning in seconds

    private Transform target;
    private float despawnTimer;

    private void Start()
    {
        despawnTimer = despawn_time;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            // Despawn if target is lost or despawn timer expires
            despawnTimer -= Time.deltaTime;
            if (despawnTimer <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * bullet_speed * Time.fixedDeltaTime);
            transform.up = direction; // Rotate the bullet to face the direction of movement
        }
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
