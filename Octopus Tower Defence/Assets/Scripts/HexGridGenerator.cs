using UnityEngine;
using UnityEngine.Tilemaps;

public class HexGridGenerator : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;
    [SerializeField] public GameObject gridPiecePrefab;

    void Start()
    {
        if (tilemap != null && gridPiecePrefab != null)
        {
            for (int q = -3; q <= 3; q++)
            {
                int r1 = Mathf.Max(-3, -q - 3);
                int r2 = Mathf.Min(3, -q + 3);
                for (int r = r1; r <= r2; r++)
                {
                    int x = q;
                    int y = r;
                    int z = -x - y;
                    Vector3Int cellPosition = new Vector3Int(x, y, z);
                    Instantiate(gridPiecePrefab, tilemap.GetCellCenterWorld(cellPosition), Quaternion.identity, tilemap.transform);
                }
            }
        }
    }
}
