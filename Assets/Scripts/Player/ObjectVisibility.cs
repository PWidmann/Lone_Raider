using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectVisibility: MonoBehaviour
{
    public static ObjectVisibility Instance;

    // Sphere check with interval, checking for objects with tag "BorderTrees"
    private int visibilityRange = 30;
    private float intervalInSeconds = 0.1f;

    private float intervalTimer = 0;

    private static GameObject[,] mapObjects;

    private static GameObject playerObject;

    private static bool playerActive = false;

    private GameObject temp;


    /// <summary>
    /// MapObjects is a GameObject array for all objects to enable/disable them
    /// </summary>
    public GameObject[,] MapObjects { get => mapObjects; set => mapObjects = value; }

    public static GameObject PlayerObject { get => playerObject; set => playerObject = value; }
    public bool PlayerActive { get => playerActive; set => playerActive = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }


    private void Update()
    {
        // Search for player gameobject for position reference
        if (!playerActive)
        {
            temp = null;
            temp = GameObject.Find("Player(Clone)");
            if (temp != null)
            {
                playerObject = temp;
                playerActive = true;
            }
        }

        if (playerActive)
        {
            // Visibility check
            intervalTimer += Time.deltaTime;
            if (intervalTimer > intervalInSeconds)
            {
                if (playerObject)
                {
                    if (DevPanel.Instance)
                    {
                        visibilityRange = DevPanel.Instance.currentVisibilityRadius;
                    }
                    else
                    {
                        visibilityRange = 30;
                    }
                    
                    intervalTimer = 0;

                    int quadPosX = 0;
                    int quadPosY = 0;
                    int quadWidth = (int)(visibilityRange * 2.2f);
                    int quadHeight = (int)quadWidth;

                    // Get search quad coordinates
                    quadPosX = (int)(playerObject.transform.position.x - visibilityRange);
                    quadPosY = (int)(playerObject.transform.position.y - visibilityRange);

                    // Loop through all coordinates within the search quad around the player and check if there is an object
                    for (int x = quadPosX; x < (quadPosX + quadWidth); x++)
                    {
                        for (int y = quadPosY; y < (quadPosY + quadHeight); y++)
                        {
                            if (IsInsideMap(x, y, 0, 0, mapObjects.GetLength(1), mapObjects.GetLength(0)))
                            {
                                if (mapObjects[y, x] != null)
                                {
                                    if (Vector3.Distance(playerObject.transform.position, new Vector3(x, y, playerObject.transform.position.z)) < visibilityRange)
                                    {
                                        mapObjects[y, x].SetActive(true);
                                    }
                                    else
                                    {
                                        mapObjects[y, x].SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                }   
            }
        }   
    }

    // look if a position is inside the width and height of a quad
    bool IsInsideMap(int _targetPosX, int _targetPosY, int _arrayPosX, int arrayPosY, int _arrayWidth, int _arrayHeight)
    {
        bool inside = false;

        if (_targetPosX >= 0 && _targetPosX < (_arrayPosX + _arrayWidth) && _targetPosY >= 0 && _targetPosY < (arrayPosY +_arrayHeight))
        {
            inside = true;
        }

        return inside;
    }

    public void DisableAllObjectsOnMap()
    {
        foreach (GameObject go in mapObjects)
        {
            if(go != null)
                go.SetActive(false);
        }
    }
}
