using System;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
  public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.directionList);
        foreach (var position in basicWallPositions)
        {
            //tilemapVisualizer.PaintSingleBasicWall(position);
            tilemapVisualizer.PaintSingleDirectionalWall(position);
        }
    }

    private static HashSet<Vector3Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();
        foreach (var position in floorPositions)
        {
            Vector2Int neighbourPosition;
            bool floorUp = true;
            bool floorDown = true;
            bool floorLeft = true;
            bool floorRight = true;

            neighbourPosition = position + Direction2D.GetUpDirection();
            if(!floorPositions.Contains(neighbourPosition))  // van felette floor?
                floorUp = false;

            neighbourPosition = position + Direction2D.GetDownDirection();
            if (!floorPositions.Contains(neighbourPosition))  // van alatta floor?
                floorDown = false;

            neighbourPosition = position + Direction2D.GetLeftDirection();
            if (!floorPositions.Contains(neighbourPosition))  // van balra floor?
                floorLeft = false;

            neighbourPosition = position + Direction2D.GetRightDirection();
            if (!floorPositions.Contains(neighbourPosition))  // van jobbra floor?
                floorRight = false;

            //if(!floorUp || !floorDown || !floorRight || !floorLeft)
            //    wallPositions.Add(position);

            // pozíciók
            //          0.   1.   2.
            //          3.   4.   5.
            //          6.   7.   8.

            if(!floorUp && !floorLeft)      // bal fent van ==> 0.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 0 });
            }
            else if (!floorUp && !floorRight)       // jobb fent van ==> 2.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 2 });
            }
            else if(!floorDown && !floorLeft)       // bal lent van ==> 6.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 6 });
            }
            else if(!floorDown && !floorRight)      // jobb lent van ==> 8.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 8 });
            }
            else if(!floorUp)       // fent középen van ==> 1.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 1 });
            }
            else if(!floorDown)     // lent középen van ==> 7.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 7 });
            }
            else if (!floorLeft)        // bal középen van ==> 3.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 3 });
            }
            else if(!floorRight)        // jobb középen van ==> 5.
            {
                wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 5 });
            }
            //else        // középen van ==> 4.
            //{
            //    wallPositions.Add(new Vector3Int { x = position.x, y = position.y, z = 4 });
            //}
        }
        return wallPositions;
    }
}
