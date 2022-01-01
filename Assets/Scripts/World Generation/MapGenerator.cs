using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] BiomeScriptableObject[] biomes = new BiomeScriptableObject[2];
    [SerializeField] Tile emptyTile;

    [Header("Player Prefab")]
    [SerializeField] GameObject playerPrefab;

    private PerlinNoise noise;
    private float[,] noiseValues;
    private float[,] falloffMap;
    private int[,] finalMapArray;

    bool playerSpawned = false;

    private int seed;
    private SaveData saveData;

    private Vector2 playerStart;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (GameManager.CreateNewWorld == true)
        {
            CreateBiomes();

            

            Debug.Log("Created new world '" + GameManager.NewWorldName + "' and saved world to file");
            GameManager.CurrentWorldName = GameManager.NewWorldName;
            GameManager.CreateNewWorld = false;
            GameManager.NewWorldName = "";
        }

        if (GameManager.LoadWorld == true)
        {
            // Get save data
            saveData = SaveSystem.LoadWorld(GameManager.LoadWorldName);
            Debug.Log("Savegame '" + saveData.gameName + "' loaded");

            GameManager.LoadWorld = false;
            GameManager.LoadWorldName = "";

            GameManager.CurrentWorldName = saveData.gameName;
            GameManager.CurrentSeed = saveData.seed;
            GameManager.CurrentMapArray = saveData.biomeMapArray;
            
            LoadBiomes(saveData.biomeMapArray);

            LoadPlayerPos(saveData.playerXpos, saveData.playerYpos);
            
        }
    }

    

    void CreateBiomes()
    {
        // Add biomes (scriptable objects) in the mapgenerator component in the main scene
        foreach (BiomeScriptableObject biome in biomes)
        {
            ////
            /// Main algorithm how biomes look

            seed = Random.Range(0, 5000);
            // Create perlin noise with random seed
            noise = new PerlinNoise(seed, 8, 4f, 1f, 1.3f, 6);
            falloffMap = FalloffGenerator.GenerateFalloffMap(biome.biomeWidth, biome.biomeHeight, 8f, 3f);
            finalMapArray = new int[biome.biomeWidth, biome.biomeHeight];

            // Reset visibility array
            ObjectVisibility.Instance.MapObjects = new GameObject[biome.biomeHeight, biome.biomeWidth];

            // Create biome objects and add scripts for the Unity tilemap system
            GameObject biomeObject = new GameObject(biome.biomeName);
            biomeObject.transform.parent = transform;

            // Basic floor tilemap
            GameObject groundTileMapObject = new GameObject("GroundTileMap");
            groundTileMapObject.transform.parent = biomeObject.transform;
            groundTileMapObject.AddComponent<Tilemap>();
            groundTileMapObject.AddComponent<TilemapRenderer>().sortingOrder = -20000;

            // Collision map
            GameObject borderTileMapObject = new GameObject("BorderTileMap");
            borderTileMapObject.transform.parent = biomeObject.transform;
            borderTileMapObject.AddComponent<Tilemap>();
            borderTileMapObject.AddComponent<TilemapRenderer>();
            borderTileMapObject.AddComponent<Rigidbody2D>().gravityScale = 0;
            borderTileMapObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            borderTileMapObject.GetComponent<Rigidbody2D>().freezeRotation = true;
            borderTileMapObject.AddComponent<CompositeCollider2D>();
            borderTileMapObject.AddComponent<TilemapCollider2D>().usedByComposite = true;

            // Set size of map objects
            Tilemap groundTileMap = groundTileMapObject.GetComponent<Tilemap>();
            Tilemap borderTileMap = borderTileMapObject.GetComponent<Tilemap>();
            groundTileMap.size = new Vector3Int(biome.biomeWidth, biome.biomeHeight, 1);
            borderTileMap.size = new Vector3Int(biome.biomeWidth, biome.biomeHeight, 1);
            noiseValues = noise.GetNoiseValues(biome.biomeWidth, biome.biomeHeight);


            // Fill final map array with noise values
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    noiseValues[x, y] = Mathf.Clamp01(noiseValues[x, y] - falloffMap[x, y]);

                    if (noiseValues[x, y] > 0.35f)
                    {
                        finalMapArray[x, y] = 1; // 1 = grass
                    }
                    else
                    {
                        finalMapArray[x, y] = 0; // 0 = border
                    }
                }
            }

            playerStart = new Vector2Int(biomes[0].biomeWidth / 3, biomes[0].biomeHeight / 3);
            

            // Here code to fill out the map with houses, npcs and grass patches



            
            
            

            // set grass tiles
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    Tile currentTile = GetRandomGroundTile(biome);
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), currentTile);
                }
            }

            // set border collision
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    if (finalMapArray[x, y] == 0)
                        borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                }
            }

            ////
            //// 0 = border
            //// 1 = walkable
            //// 2 = tree1
            //// 3 = tree2
            //// 4 = tree3
            //// 5 = bush1
            //// 6 = bush2
            ////
            
            GameObject plants = new GameObject("Border Plants");
            plants.transform.parent = biomeObject.transform;

            // place big trees
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    if (finalMapArray[x, y] == 0) // 0 is border
                    {
                        // 70% chance to spawn a tree, not next to other trees
                        if (Random.Range(0, 10) > 7 && !IsNextToObject(x, y, 2, biome)) 
                        {
                            int rnd = Random.Range(0, biome.bigBorderObjects.Length);
                            GameObject tree = Instantiate(biome.bigBorderObjects[rnd]);
                            tree.transform.parent = plants.transform;
            
                            tree.gameObject.transform.position = new Vector3(x + 0.5f, y + 1.5f, -1f);

                            switch (rnd)
                            {
                                case 0:
                                    finalMapArray[x, y] = 2;
                                    break;
                                case 1:
                                    finalMapArray[x, y] = 3;
                                    break;
                                case 2:
                                    finalMapArray[x, y] = 4;
                                    break;
                            }

                            ObjectVisibility.Instance.MapObjects[y, x] = tree;
                        }
                    }
                }
            }

            // Fill up border with bushes
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    if (finalMapArray[x, y] == 0) // 0 is border
                    {
                        int rnd = Random.Range(0, biome.littleBorderObjects.Length);
                        GameObject plant = Instantiate(biome.littleBorderObjects[rnd]);
                        plant.transform.parent = plants.transform;
                        plant.gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1f);
                        

                        switch (rnd)
                        {
                            case 0:
                                finalMapArray[x, y] = 5;
                                break;
                            case 1:
                                finalMapArray[x, y] = 6;
                                break;
                            case 2:
                                finalMapArray[x, y] = 7;
                                break;
                        }

                        ObjectVisibility.Instance.MapObjects[y, x] = plant;
                    }
                }
            }

            GameManager.CurrentMapArray = finalMapArray;

            SaveWorld();
            SpawnPlayer();
            // Place wiggle grass
            //for (int x = 0; x < biome.biomeWidth; x++)
            //{
            //    for (int y = 0; y < biome.biomeHeight; y++)
            //    {
            //        if (finalMapArray[x, y] == 1) // 1 is walkable
            //        {
            //            int rnd = Random.Range(0, biome.walkbyWiggleGrass.Length);
            //            GameObject grass = Instantiate(biome.walkbyWiggleGrass[rnd]);
            //            grass.transform.parent = plants.transform;
            //            grass.gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1f);
            //            finalMapArray[x, y] = 4; // 4 = wiggle grass walkable
            //
            //            ObjectVisibility.Instance.MapObjects[y, x] = grass;
            //        }
            //    }
            //}

            if (biome.biomeName == "Desert")
            {
                biomeObject.transform.position += new Vector3(100, 0, 0);
            }
        }
    }

    void LoadBiomes(int[,] _loadedMapArray)
    {
        foreach (BiomeScriptableObject biome in biomes)
        {          
            finalMapArray = new int[biome.biomeWidth, biome.biomeHeight];
            finalMapArray = _loadedMapArray;

            ObjectVisibility.Instance.MapObjects = new GameObject[biome.biomeHeight, biome.biomeWidth];

            GameObject biomeObject = new GameObject(biome.biomeName);
            biomeObject.transform.parent = transform;


            GameObject groundTileMapObject = new GameObject("GroundTileMap");
            groundTileMapObject.transform.parent = biomeObject.transform;
            groundTileMapObject.AddComponent<Tilemap>();
            groundTileMapObject.AddComponent<TilemapRenderer>().sortingOrder = -20000;

            GameObject borderTileMapObject = new GameObject("BorderTileMap");
            borderTileMapObject.transform.parent = biomeObject.transform;
            borderTileMapObject.AddComponent<Tilemap>();
            borderTileMapObject.AddComponent<TilemapRenderer>();
            borderTileMapObject.AddComponent<Rigidbody2D>().gravityScale = 0;
            borderTileMapObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            borderTileMapObject.GetComponent<Rigidbody2D>().freezeRotation = true;
            borderTileMapObject.AddComponent<CompositeCollider2D>();
            borderTileMapObject.AddComponent<TilemapCollider2D>().usedByComposite = true;


            Tilemap groundTileMap = groundTileMapObject.GetComponent<Tilemap>();
            Tilemap borderTileMap = borderTileMapObject.GetComponent<Tilemap>();
            groundTileMap.size = new Vector3Int(biome.biomeWidth, biome.biomeHeight, 1);
            borderTileMap.size = new Vector3Int(biome.biomeWidth, biome.biomeHeight, 1);

            // set grass tiles
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    Tile currentTile = GetRandomGroundTile(biome);
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), currentTile);
                }
            }

            // set border collision
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    if (finalMapArray[x, y] == 0)
                        borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                }
            }


            GameObject plants = new GameObject("Border Plants");
            plants.transform.parent = biomeObject.transform;

            // Place map objects
            for (int x = 0; x < biome.biomeWidth; x++)
            {
                for (int y = 0; y < biome.biomeHeight; y++)
                {
                    switch (finalMapArray[x, y])
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            GameObject tree = Instantiate(biome.bigBorderObjects[0]);
                            tree.transform.parent = plants.transform;
                            tree.gameObject.transform.position = new Vector3(x + 0.5f, y + 1.5f, -1f);
                            ObjectVisibility.Instance.MapObjects[y, x] = tree;
                            borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                            break;
                        case 3:
                            GameObject tree2 = Instantiate(biome.bigBorderObjects[1]);
                            tree2.transform.parent = plants.transform;
                            tree2.gameObject.transform.position = new Vector3(x + 0.5f, y + 1.5f, -1f);
                            ObjectVisibility.Instance.MapObjects[y, x] = tree2;
                            borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                            break;
                        case 4:
                            GameObject tree3 = Instantiate(biome.bigBorderObjects[2]);
                            tree3.transform.parent = plants.transform;
                            tree3.gameObject.transform.position = new Vector3(x + 0.5f, y + 1.5f, -1f);
                            ObjectVisibility.Instance.MapObjects[y, x] = tree3;
                            borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                            break;
                        case 5:
                            GameObject plant = Instantiate(biome.littleBorderObjects[0]);
                            plant.transform.parent = plants.transform;
                            plant.gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1f);
                            ObjectVisibility.Instance.MapObjects[y, x] = plant;
                            borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                            break;
                        case 6:
                            GameObject plant2 = Instantiate(biome.littleBorderObjects[1]);
                            plant2.transform.parent = plants.transform;
                            plant2.gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1f);
                            ObjectVisibility.Instance.MapObjects[y, x] = plant2;
                            borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyTile);
                            break;
                        case 7:
                            break;
                    }
                }
            }


            // place big trees
            //for (int x = 0; x < biome.biomeWidth; x++)
            //
            //   for (int y = 0; y < biome.biomeHeight; y++)
            //   {
            //       if (finalMapArray[x, y] == 0) // 0 is border
            //       {
            //           if (Random.Range(0, 10) > 7 && !IsNextToObject(x, y, 2, biome))
            //           {
            //               int index = _loadedMapArray[x,y]
            //
            //               int rnd = Random.Range(0, biome.bigBorderObjects.Length);
            //               GameObject tree = Instantiate(biome.bigBorderObjects[rnd]);
            //               tree.transform.parent = plants.transform;
            //
            //               tree.gameObject.transform.position = new Vector3(x + 0.5f, y + 1.5f, -1f);
            //               finalMapArray[x, y] = 2; // 2 = tree
            //
            //               ObjectVisibility.Instance.MapObjects[y, x] = tree;
            //           }
            //       }
            //   }
            //

            // Fill up border with little obstacles
            //for (int x = 0; x < biome.biomeWidth; x++)
            //{
            //    for (int y = 0; y < biome.biomeHeight; y++)
            //    {
            //        if (finalMapArray[x, y] == 0) // 0 is border
            //        {
            //            int rnd = Random.Range(0, biome.littleBorderObjects.Length);
            //            GameObject plant = Instantiate(biome.littleBorderObjects[rnd]);
            //            plant.transform.parent = plants.transform;
            //            plant.gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1f);
            //            finalMapArray[x, y] = 3; // 3 = clutter obstacles
            //            ObjectVisibility.Instance.MapObjects[y, x] = plant;
            //        }
            //    }
            //}

            // Place wiggle grass
            //for (int x = 0; x < biome.biomeWidth; x++)
            //{
            //    for (int y = 0; y < biome.biomeHeight; y++)
            //    {
            //        if (finalMapArray[x, y] == 1) // 1 is walkable
            //        {
            //            int rnd = Random.Range(0, biome.walkbyWiggleGrass.Length);
            //            GameObject grass = Instantiate(biome.walkbyWiggleGrass[rnd]);
            //            grass.transform.parent = plants.transform;
            //            grass.gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1f);
            //            finalMapArray[x, y] = 4; // 4 = wiggle grass walkable
            //
            //            ObjectVisibility.Instance.MapObjects[y, x] = grass;
            //        }
            //    }
            //}

            if (biome.biomeName == "Desert")
            {
                biomeObject.transform.position += new Vector3(100, 0, 0);
            }
        }
    }

    private void SaveWorld()
    {
        SaveSystem.SaveWorld(GameManager.NewWorldName, GameManager.CurrentMapArray, seed, playerStart.x, playerStart.y);
    }


    void SpawnPlayer()
    {
        if (!playerSpawned)
        {
            // Spawn somewhat in the middle of the map
            for (int x = 30; x < GameManager.CurrentMapArray.GetLength(0) -30 ; x++)
            {
                for (int y = 30; y < GameManager.CurrentMapArray.GetLength(1) -30; y++)
                {
                    if (GameManager.CurrentMapArray[x, y] == 1) // Walkable grass
                    {
                        if (!IsNextToObject(x, y, 0, biomes[0]))
                        {
                            GameObject player = Instantiate(playerPrefab);
                            player.gameObject.transform.position = new Vector3(x + 0.5f, y + 0.5f, -1);

                            playerSpawned = true;
                            playerStart.x = x;
                            playerStart.y = y;
                            cam.transform.position = new Vector3(x, y, -3.861016f);

                            return;
                        }
                    }
                }
            }
            
        }
    }

    void LoadPlayerPos(float xPos, float yPos)
    {
        if (!playerSpawned)
        {
            GameObject player = Instantiate(playerPrefab);
            player.gameObject.transform.position = new Vector3(xPos + 0.5f, yPos + 0.5f, -1);
            playerSpawned = true;
            playerStart.x = xPos;
            playerStart.y = yPos;

            cam.transform.position = new Vector3(xPos, yPos, -3.861016f);
        }
    }

    bool IsNextToObject(int xPos, int yPos, int mapObjectIndex, BiomeScriptableObject sObject)
    {
        if (IsInside2DArray(xPos + 1, yPos, sObject) && finalMapArray[xPos + 1, yPos] == mapObjectIndex
            || IsInside2DArray(xPos + 1, yPos + 1, sObject) && finalMapArray[xPos + 1, yPos + 1] == mapObjectIndex
            || IsInside2DArray(xPos, yPos + 1, sObject) && finalMapArray[xPos, yPos + 1] == mapObjectIndex
            || IsInside2DArray(xPos - 1, yPos + 1, sObject) && finalMapArray[xPos - 1, yPos + 1] == mapObjectIndex
            || IsInside2DArray(xPos - 1, yPos, sObject) && finalMapArray[xPos - 1, yPos] == mapObjectIndex
            || IsInside2DArray(xPos - 1, yPos - 1, sObject) && finalMapArray[xPos - 1, yPos - 1] == mapObjectIndex
            || IsInside2DArray(xPos, yPos - 1, sObject) && finalMapArray[xPos, yPos - 1] == mapObjectIndex
            || IsInside2DArray(xPos + 1, yPos - 1, sObject) && finalMapArray[xPos + 1, yPos - 1] == mapObjectIndex
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Tile GetRandomGroundTile(BiomeScriptableObject sObject)
    {
        int rnd = Random.Range(0, sObject.groundMapTiles.Length);
        Tile tile;
        tile = sObject.groundMapTiles[rnd];

        return tile;
    }

    bool IsInside2DArray(int xPos, int yPos, BiomeScriptableObject sObject)
    {
        bool inside = false;

        if (xPos >= 0 && xPos < sObject.biomeWidth && yPos >= 0 && yPos < sObject.biomeHeight)
        {
            inside = true;
        }

        return inside;
    }

}
