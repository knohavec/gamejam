using UnityEngine;
using System;
using System.Collections;

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
    public Color damageColor = new Color(1f, 0f, 0f, 0.5f);
    public CurrencyType currencyType;
    [SerializeField] private Color radiusColor = Color.green;
    public float flashDuration = 3.0f; // Duration to stop flashing if no damage is taken

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine damageFlashCoroutine;
    private Coroutine stopFlashCoroutine;
    private Tile parentTile;
    private bool isRadiusVisible = false;
    private GameObject attackRadiusCircle;

    public static event Action<Tower> OnTowerDestroyed;

    public enum CurrencyType
    {
        SandDollars,
        Pollutium
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        parentTile = GetComponentInParent<Tile>();
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
                // Reset the stop flash coroutine to ensure the flashing continues until the delay elapses
                if (stopFlashCoroutine != null)
                {
                    StopCoroutine(stopFlashCoroutine);
                }
                stopFlashCoroutine = StartCoroutine(StopFlashingAfterDelay());
            }
        }
    }

    public void SetParentTile(Tile tile)
    {
        parentTile = tile;
        Debug.Log("Parent tile set for the tower.");
    }

    private void DestroyTower()
    {
        OnTowerDestroyed?.Invoke(this); // Notify subscribers that the tower is destroyed
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
            }
        }

        if (stopFlashCoroutine != null)
        {
            StopCoroutine(stopFlashCoroutine);
            stopFlashCoroutine = null;
        }
    }

    private IEnumerator DamageFlash(float attackSpeed)
    {
        float flashDuration = attackSpeed * 0.5f;
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
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private IEnumerator StopFlashingAfterDelay()
    {
        yield return new WaitForSeconds(flashDuration);

        if (damageFlashCoroutine != null)
        {
            StopCoroutine(damageFlashCoroutine);
            damageFlashCoroutine = null;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
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
