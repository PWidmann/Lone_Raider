using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string gameName;
    public int[,] biomeMapArray;
    public int seed;
    public int playerXpos;
    public int playerYpos;


    public SaveData(string _gameName, int[,] _mapArray, int _seed, int _playerX, int _playerY)
    {
        gameName = _gameName;
        biomeMapArray = _mapArray;
        seed = _seed;
        playerXpos = _playerX;
        playerYpos = _playerY;
    }
}