using System.Collections;
using UnityEngine;
using System;


[Serializable]
public class Tower : MonoBehaviour
{
    public string towername;
    public int towercost;
    public GameObject towerprefab;
    public int towerhealth;
    public bool isDestroyed = false;
    public float tower_attack_range;
    public int towerdamage;
    public float tower_attack_speed;
    public int tower_research_cost;
    public Color damageColor = Color.red; // Selectable damage color
    public float flashDuration = 0.1f; // Duration of the flash effect
    public CurrencyType currencyType; // Add this line to define the currency type

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine damageFlashCoroutine;
    private bool isBeingAttacked = false;

    public enum CurrencyType
    {
        SandDollars,
        Pollutium
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public Tower (string _name, float _attackspeed, int _research_cost, int _damage, int _cost, GameObject _prefab, int _health, float _attackrange, CurrencyType _currencyType){
        towername = _name;
        towercost = _cost;
        towerprefab = _prefab;
        tower_attack_range = _attackrange;
        towerhealth = _health;
        towerdamage = _damage;
        tower_attack_speed = _attackspeed;
        tower_research_cost = _research_cost;
        currencyType = _currencyType;
    }

    public void TakeDamage(int dmg, float attackSpeed)
    {
        towerhealth -= dmg;
        isBeingAttacked = true;
        if (spriteRenderer != null)
        {
            if (damageFlashCoroutine == null)
            {
                damageFlashCoroutine = StartCoroutine(DamageFlash(attackSpeed));
            }
        }

        if (towerhealth <= 0)
        {
            // Stop the attack and the damage flash coroutine
            StopAttack();
            Destroy(gameObject);
            isDestroyed = true;
        }
    }

    public void StopAttack()
    {
        isBeingAttacked = false;
        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
            spriteRenderer.color = originalColor;
            damageFlashCoroutine = null;
        }
    }

    private IEnumerator DamageFlash(float attackSpeed)
    {
        while (isBeingAttacked)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(attackSpeed - flashDuration);
        }
        damageFlashCoroutine = null;
    }
}
