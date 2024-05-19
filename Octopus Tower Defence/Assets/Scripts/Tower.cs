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
    public CurrencyType currencyType;
    [SerializeField] private Color radiusColor = Color.green; // Color of the attack radius

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine damageFlashCoroutine;
    private Tile parentTile;
    private bool isRadiusVisible = false;
    private GameObject attackRadiusCircle;

    public enum CurrencyType
    {
        SandDollars,
        Pollutium
    }

    void Start()
    {
        // First, try to get the SpriteRenderer from the parent object
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If not found in the parent object, try to get it from the children
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        parentTile = GetComponentInParent<Tile>();

        // Create the attack radius circle
        CreateAttackRadiusCircle();
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
                DestroyTower();
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

    private void DestroyTower()
    {
        if (parentTile != null)
        {
            parentTile.SetTowerPresence(false);
        }
        Destroy(gameObject);
    }

    public void StopAttack()
    {
        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
            damageFlashCoroutine = null;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
                Debug.Log("Stopped DamageFlash coroutine and reset color");
            }
        }
    }

    private IEnumerator DamageFlash(float attackSpeed)
    {
        float flashDuration = attackSpeed * 0.5f; // Adjust the flash duration based on the attack speed
        while (!isDestroyed)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = damageColor;
            }
            yield return new WaitForSeconds(flashDuration);
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            yield return new WaitForSeconds(attackSpeed - flashDuration);
        }
    }

    void OnMouseDown()
    {
        ToggleAttackRadius();
    }

    private void CreateAttackRadiusCircle()
    {
        attackRadiusCircle = new GameObject("AttackRadiusCircle");
        attackRadiusCircle.transform.SetParent(transform);
        attackRadiusCircle.transform.localPosition = Vector3.zero;

        var meshFilter = attackRadiusCircle.AddComponent<MeshFilter>();
        var meshRenderer = attackRadiusCircle.AddComponent<MeshRenderer>();

        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = radiusColor;

        meshFilter.mesh = CreateCircleMesh(tower_attack_range);

        attackRadiusCircle.SetActive(false);
    }

    private Mesh CreateCircleMesh(float radius)
    {
        int segments = 100;

        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];
        Color[] colors = new Color[vertices.Length];

        vertices[0] = Vector3.zero;
        colors[0] = radiusColor;

        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / (float)segments * Mathf.PI * 2f;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            colors[i + 1] = radiusColor;

            if (i < segments - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
            else
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void ToggleAttackRadius()
    {
        isRadiusVisible = !isRadiusVisible;
        attackRadiusCircle.SetActive(isRadiusVisible);
    }

    public void StopFlashing()
    {
        StopAttack();
    }
}
