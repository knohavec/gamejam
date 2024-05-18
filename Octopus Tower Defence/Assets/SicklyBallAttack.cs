// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Tilemaps;

// public class SicklyBallAttack : MonoBehaviour
// {
//     public Animator animator;
//     public LayerMask enemyLayer;

//     private Tower tower;
//     private bool isAttacking = false;
//     private Tilemap tilemap;

//     void Start()
//     {
//         tower = GetComponent<Tower>();
//         if (tower == null)
//         {
//             Debug.LogError("Tower component not found on the GameObject.");
//         }

//         tilemap = FindObjectOfType<Tilemap>();
//         if (tilemap == null)
//         {
//             Debug.LogError("Tilemap component not found in the scene.");
//         }
//     }

//     void Update()
//     {
//         if (!isAttacking)
//         {
//             CheckForEnemies();
//         }
//     }

//     private void CheckForEnemies()
//     {
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, tower.tower_attack_range, enemyLayer);
//         if (hitEnemies.Length > 0)
//         {
//             isAttacking = true;
//             animator.SetBool("IsExploding", true);
//         }
//     }

//     // This method is called at the end of the explosion animation
//     public void OnAttackComplete()
//     {
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, tower.tower_attack_range, enemyLayer);
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             Enemy enemyScript = enemy.GetComponent<Enemy>();
//             if (enemyScript != null)
//             {
//                 enemyScript.TakeDamage(tower.towerdamage);
//             }
//         }

//         // Destroy the RuleTile on the Tilemap
//         Vector3Int tilePosition = tilemap.WorldToCell(transform.position);
//         tilemap.SetTile(tilePosition, null);

//         // Destroy the sickly ball object
//         Destroy(gameObject);
//     }

//     void OnDrawGizmosSelected()
//     {
//         if (tower != null)
//         {
//             Gizmos.color = Color.red;
//             Gizmos.DrawWireSphere(transform.position, tower.tower_attack_range);
//         }
//     }
// }
