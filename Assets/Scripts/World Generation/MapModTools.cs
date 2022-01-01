using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapModTools
{
    public static int[,] SpawnEnemies(int[,] _mapObjectsArray, int[,] _baseMapArray)
    {
        int[,] outputArray = new int[_mapObjectsArray.GetLength(1),_mapObjectsArray.GetLength(0)];

        

        //int enemyCount = 10;

        // 0 = border
        // 1 = walkable

        // Look for walkable space in _baseMapArray then look in _mapObjectsArray if position is already taken by an object




        return outputArray;
    }

    public static int[,] GetClearObjectMapArray(int[,] _mapObjectArray)
    {
        int[,] output = new int[_mapObjectArray.GetLength(1), _mapObjectArray.GetLength(0)];

        return output;
    }
}
