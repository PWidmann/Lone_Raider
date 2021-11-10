using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static bool createNewWorld = false;
    private static bool loadWorld = false;
    private static string newWorldName = "";
    private static string loadWorldName = "";
    private static string currentWorldName = "";

    private static int currentSeed = 0;
    private static int[,] currentMapArray;


    public static bool CreateNewWorld { get => createNewWorld; set => createNewWorld = value; }
    public static string NewWorldName { get => newWorldName; set => newWorldName = value; }
    public static bool LoadWorld { get => loadWorld; set => loadWorld = value; }
    public static string LoadWorldName { get => loadWorldName; set => loadWorldName = value; }
    public static string CurrentWorldName { get => currentWorldName; set => currentWorldName = value; }
    public static int CurrentSeed { get => currentSeed; set => currentSeed = value; }
    public static int[,] CurrentMapArray { get => currentMapArray; set => currentMapArray = value; }
}
