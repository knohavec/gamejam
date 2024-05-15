using UnityEngine;
using UnityEngine.Tilemaps;

public class GridPieceTile : TileBase
{
    public GameObject gridPiecePrefab;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = gridPiecePrefab.GetComponent<SpriteRenderer>().sprite;
        tileData.gameObject = gridPiecePrefab;
        tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
        tileData.colliderType = Tile.ColliderType.Sprite;
    }
}
