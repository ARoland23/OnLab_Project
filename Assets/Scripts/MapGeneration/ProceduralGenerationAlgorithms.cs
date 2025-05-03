using System.Collections.Generic;
using System.Linq;
using Unity.Transforms;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.getRandomDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit,int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);
        while(roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth) 
            {
                if(Random.value < 0.5f)
                {

                    if(room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(room, minWidth, minHeight, roomsQueue);
                    }
                    else if(room.size.x >= minWidth * 2)
                    {
                        SplitVertically(room, minWidth, minHeight, roomsQueue);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }

                }
                else 
                {
                    if ( room.size.x >= minWidth * 2)
                    {
                        SplitVertically(room, minWidth, minHeight, roomsQueue);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(room, minWidth, minHeight, roomsQueue);
                    }
                    else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        return roomsList;
    }

    private static void SplitVertically(BoundsInt room, int minWidth, int minHeight, Queue<BoundsInt> roomsQueue)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt( new Vector3Int(room.min.x + xSplit, room.min.y,room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y,room.size.z) );
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(BoundsInt room, int minWidth, int minHeight, Queue<BoundsInt> roomsQueue)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.y, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public class Direction2D
{
    public static List<Vector2Int> directionList = new List<Vector2Int>()
    {
        new Vector2Int(0,1),    // up
        new Vector2Int(1, 0),   // right
        new Vector2Int(0, -1),  // down
        new Vector2Int(-1, 0),  // left
    };

    public static Vector2Int getRandomDirection()
    {
        return directionList[Random.Range(0, directionList.Count)];
    }

    public static Vector2Int GetUpDirection() { return directionList[0]; }
    public static Vector2Int GetDownDirection() { return directionList[2]; }
    public static Vector2Int GetLeftDirection() { return directionList[3]; }
    public static Vector2Int GetRightDirection() { return directionList[1]; }
}
