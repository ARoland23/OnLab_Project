using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] public Tilemap floorTilemap, wallTilemap;
    [SerializeField] public TilemapVisualizerSO tilemapVisualizerSO;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, tilemapVisualizerSO.floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(position,tilemap,tile);
        }  
    }

    private void PaintSingleTile(Vector2Int position, Tilemap tilemap, TileBase tile)
    {
        var tilePosition = tilemap.WorldToCell( (Vector3Int)position );
        tilemap.SetTile(tilePosition, tile);
    }
    private void PaintSingleDirectionalTile(Vector3Int position, Tilemap tilemap, TileBase[] tiles)
    {
        Vector3Int worldPosition = Vector3Int.zero;
        worldPosition.x = position.x;
        worldPosition.y = position.y;
        worldPosition.z = 0;
        var tilePosition = tilemap.WorldToCell(worldPosition);
        tilemap.SetTile(tilePosition, tiles[position.z]);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    public void PaintSingleBasicWall(Vector2Int position)
    {
        PaintSingleTile(position, wallTilemap, tilemapVisualizerSO.wallTop);
    }
    public void PaintSingleDirectionalWall(Vector3Int position)
    {
        PaintSingleDirectionalTile(position, wallTilemap, tilemapVisualizerSO.tiles);
    }
}
