using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstMapGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] private int minRoomWidth = 4;
    [SerializeField] private int minRoomHeight = 4;
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;

    [SerializeField] private int offset = 0;

    [SerializeField] private bool RandomWalkRooms = false;
    [SerializeField] private TilemapVisualizerSO[] tilemapVisualizerSOs;

   // private List<BoundsInt> roomsList;

    protected override void RunProceduralGeneration()
    {
        objectPlacer.Clear();
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning( new BoundsInt((Vector3Int)startPosition,new Vector3Int(mapWidth, mapHeight, 0)), minRoomWidth, minRoomHeight);
        tilemapVisualizer.tilemapVisualizerSO = tilemapVisualizerSOs[Random.Range(0, tilemapVisualizerSOs.Length)];
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);
        //floor = CreateDistinctRooms(roomsList,tilemapVisualizerSOs);
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        //Vector2Int playerRoomPosition = new Vector2Int(int.MaxValue,int.MaxValue);
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));

            //Vector2Int currentRoomPosition = Vector2Int.RoundToInt(room.center);
            //if (currentRoomPosition.y < playerRoomPosition.y)
            //    playerRoomPosition = currentRoomPosition;
        }
        var playerRoomPosition = roomCenters.OrderBy(center => center.y).First();
        var player = GameObject.FindWithTag("Player");
        player.transform.position = new Vector3(playerRoomPosition.x, playerRoomPosition.y, 0);

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        objectPlacer.PlaceEnemies(roomsList,playerRoomPosition);
        objectPlacer.PlaceHealth(roomsList,playerRoomPosition);
    }

    

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    { 
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);                                                  // egy összekötés van

        while(roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter,closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor  = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);


        // keressük fel-le
        while (position.y != destination.y)
        {
            if(destination.y > currentRoomCenter.y) 
            {
                position += Direction2D.GetUpDirection();
            }
            else if (destination.y < currentRoomCenter.y)
            {
                position += Direction2D.GetDownDirection();
            }
            corridor.Add(position);
            corridor.Add(position + Direction2D.GetLeftDirection());
            corridor.Add(position + Direction2D.GetRightDirection());
        }
        // egyel továb megyünk
        var positionOneMoreUp = position + Direction2D.GetUpDirection();
        var positionOneMoreDown = position + Direction2D.GetDownDirection();
        corridor.Add(positionOneMoreUp);
        corridor.Add(positionOneMoreUp + Direction2D.GetLeftDirection());
        corridor.Add(positionOneMoreUp + Direction2D.GetRightDirection());
        corridor.Add(positionOneMoreDown);
        corridor.Add(positionOneMoreDown + Direction2D.GetLeftDirection());
        corridor.Add(positionOneMoreDown + Direction2D.GetRightDirection());


        // keressük jobbra-balra
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Direction2D.GetRightDirection();
            }
            else if (destination.x < position.x)
            {
                position += Direction2D.GetLeftDirection();
            }
            corridor.Add(position);
            corridor.Add(position + Direction2D.GetUpDirection());
            corridor.Add(position + Direction2D.GetDownDirection());
        }

        // egyel továb megyünk
        var positionOneMoreLeft = position + Direction2D.GetLeftDirection();
        var positionOneMoreRight = position + Direction2D.GetRightDirection();
        corridor.Add(positionOneMoreLeft);
        corridor.Add(positionOneMoreLeft + Direction2D.GetUpDirection());
        corridor.Add(positionOneMoreLeft + Direction2D.GetDownDirection());
        corridor.Add(positionOneMoreRight);
        corridor.Add(positionOneMoreRight + Direction2D.GetUpDirection());
        corridor.Add(positionOneMoreRight + Direction2D.GetDownDirection());
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach(var position in roomCenters)
        {
            float currentDistance = Vector2Int.Distance(position,currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> CreateDistinctRooms(List<BoundsInt> roomsList, TilemapVisualizerSO[] tilemapVisualizerSOs)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            tilemapVisualizer.tilemapVisualizerSO = tilemapVisualizerSOs[Random.Range(0, tilemapVisualizerSOs.Length)];

            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>();
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    roomFloor.Add(position);
                }
            }
            WallGenerator.CreateWalls(floor, tilemapVisualizer);
            tilemapVisualizer.PaintFloorTiles(roomFloor);
            floor.UnionWith(roomFloor);
        }
        return floor;
    }
}
