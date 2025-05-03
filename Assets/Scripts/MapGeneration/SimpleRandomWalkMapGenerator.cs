using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkMapGenerator : AbstractMapGenerator
{
    [SerializeField] private SimpleRandomWalkSO randomWalkParameters;


    protected override void RunProceduralGeneration()
    {
        tilemapVisualizer.Clear();
        HashSet<Vector2Int> floorpositions = RunRandomWalk();
        tilemapVisualizer.PaintFloorTiles(floorpositions);
        WallGenerator.CreateWalls(floorpositions,tilemapVisualizer);
        foreach (var floor in floorpositions)
        {
            Debug.Log(floor);
        }
    }

    private HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < randomWalkParameters.iterations; i++) 
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
            floorPositions.UnionWith(path);
            if (randomWalkParameters.startRandomly)
                currentPosition = floorPositions.ElementAt(Random.Range(0,floorPositions.Count));
        }
        return floorPositions;
    }
}
