using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapModTools
{
    /// <summary>
    /// Initialize the array with 0s | 0 = border
    /// </summary>
    /// <param name="_mapObjectArray"></param>
    /// <returns></returns>
    public static int[,] GetClearObjectMapArray(int[,] _mapObjectArray)
    {
        int[,] output = new int[_mapObjectArray.GetLength(1), _mapObjectArray.GetLength(0)];

        return output;
    }

    /// <summary>
    /// Fill the map with grass = 1
    /// </summary>
    /// <param name="_mapObjectArray"></param>
    /// <returns></returns>
    public static int[,] MakeMapAllGrass(int[,] _mapObjectArray)
    {
        int[,] output = _mapObjectArray;

        for (int x = 0; x < output.GetLength(1); x++)
        {
            for (int y = 0; y < output.GetLength(0); y++)
            {
                output[y, x] = 1;
            }
        }

        return output;
    }

    /// <summary>
    /// Generate 3 city areas biased to the corners of the map. City area = 2
    /// </summary>
    /// <param name="_mapObjectArray"></param>
    /// <returns></returns>
    public static int[,] CreateCityAreas(int[,] _mapObjectArray)
    {
        int[,] output = _mapObjectArray;


        Rect[] cityRects = new Rect[3];
        


        // 3 cities to generate
        for (int i = 0; i < 3; i++)
        {
            int mapWidth = _mapObjectArray.GetLength(1);
            int mapHeight = _mapObjectArray.GetLength(0);

            int cityWidth = Random.Range(30, 50);
            int cityHeight = Random.Range(30, 50);

            // 40 tiles free border
            int cityPosX = Random.Range(0 + 40, mapWidth - cityWidth - 40); 
            int cityPosY = Random.Range(0 + 40, mapHeight - cityHeight - 40);

            cityRects[i].x = cityPosX;
            cityRects[i].y = cityPosY;
            cityRects[i].width = cityWidth;
            cityRects[i].height = cityHeight;

            // Check if current city overlaps already created cities, if yes, create it again
            foreach (Rect rect in cityRects)
            {
                if (rect != null && rect != cityRects[i])
                {
                    if (rect.Overlaps(cityRects[i]))
                    {
                        i--;
                        break;
                    }
                }
            }
        }

        // Write city rects into map
        foreach (Rect cityRect in cityRects)
        {
            Debug.Log("write city into world array");
            for (int x = (int)cityRect.x; x < ((int)cityRect.x + cityRect.width); x++)
            {
                for (int y = (int)cityRect.y; y < ((int)cityRect.y + cityRect.height); y++)
                {
                    output[y, x] = 2;
                }
            }
        }

        // Testing
        //for (int x = 0; x < 40; x++)
        //{
        //    for (int y = 0; y < 40; y++)
        //    {
        //        output[y, x] = 2;
        //    }
        //}

        return output;
    }
}
