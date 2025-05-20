using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private int healthPickupCount = 0;
    [SerializeField] private GameObject enemyPistol;
    [SerializeField] private GameObject enemyRifle;
    [SerializeField] private GameObject healthPickup;
    public void PlaceEnemies(List<BoundsInt> roomsList, Vector2Int playerRoomPosition)
    {
        //if(GameLogic.difficulty == GameLogic.Difficulty.Easy)
        //    enemyCount = 2;
        //else if (GameLogic.difficulty == GameLogic.Difficulty.Medium)
        //    enemyCount = 4;
        //else if(GameLogic.difficulty == GameLogic.Difficulty.Hard)
        //    enemyCount = 5;
        enemyCount = GameLogic.enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            BoundsInt room;
            Vector3Int spawnPoint;
            Vector2Int center;
            do
            {
                room = roomsList[Random.Range(0, roomsList.Count)];
                center = Vector2Int.RoundToInt(room.center);
            }
            while (center.x == playerRoomPosition.x && center.y == playerRoomPosition.y);

            int x = Random.Range(room.xMin + 1, room.xMax - 1);
            int y = Random.Range(room.yMin + 1, room.yMax - 1);
            spawnPoint = new Vector3Int(x, y, 0);

            GameObject prefabToSpawn = Random.value < 0.5f? enemyPistol : enemyRifle;
            Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity);
        }
    }

    public void PlaceHealth(List<BoundsInt> roomsList, Vector2Int playerRoomPosition)
    {
        if (GameLogic.difficulty == GameLogic.Difficulty.Easy)
            healthPickupCount = 2;
        else if (GameLogic.difficulty == GameLogic.Difficulty.Medium)
            healthPickupCount = 1;
        else if (GameLogic.difficulty == GameLogic.Difficulty.Hard)
            return;

        for (int i = 0; i < healthPickupCount; i++)
        {
            BoundsInt room;
            Vector3Int spawnPoint;
            Vector2Int center;
            do
            {
                room = roomsList[Random.Range(0, roomsList.Count)];
                center = Vector2Int.RoundToInt(room.center);
            }
            while (center.x == playerRoomPosition.x && center.y == playerRoomPosition.y);

            int x = Random.Range(room.xMin + 1, room.xMax - 1);
            int y = Random.Range(room.yMin + 1, room.yMax - 1);
            spawnPoint = new Vector3Int(x, y, 0);

            GameObject prefabToSpawn = healthPickup;
            Instantiate(prefabToSpawn, spawnPoint, Quaternion.identity);
        }
    }

    public void Clear()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var pickups = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (var enemy in enemies)
        {
            DestroyImmediate(enemy);
        }
        foreach (var pickup in pickups)
        {
            DestroyImmediate(pickup);
        }
    }
}
