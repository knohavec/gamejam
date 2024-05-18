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
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f); // Semi-transparent red color to flash when taking damage
    public float flashDuration = 0.1f;
    public CurrencyType currencyType;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine damageFlashCoroutine;

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

    public Tower(string _name, float _attackspeed, int _research_cost, int _damage, int _cost, GameObject _prefab, int _health, float _attackrange, CurrencyType _currencyType)
    {
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
        if (!isDestroyed)
        {
            towerhealth -= dmg;
            if (towerhealth <= 0)
            {
                isDestroyed = true;
                StopAttack();
                Destroy(gameObject);
            }
            else
            {
                if (damageFlashCoroutine == null)
                {
                    damageFlashCoroutine = StartCoroutine(DamageFlash(attackSpeed));
                }
            }
        }
    }

    public void StopAttack()
    {
        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
            spriteRenderer.color = originalColor;
            damageFlashCoroutine = null;
        }
    }

    private IEnumerator DamageFlash(float attackSpeed)
    {
        while (!isDestroyed)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(attackSpeed - flashDuration);
        }
    }
}
