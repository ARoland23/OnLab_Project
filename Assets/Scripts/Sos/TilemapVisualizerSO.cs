using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TilemapVisualizerParameters_", menuName = "Map/TilemapVisualizerData")]
public class TilemapVisualizerSO : ScriptableObject
{

    [SerializeField] public TileBase floorTile, wallTop;
    [SerializeField] public TileBase[] tiles;
    [SerializeField] public TileBase[] objects;
}
