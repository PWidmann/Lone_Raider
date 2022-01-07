using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] BiomeScriptableObject[] biomes = new BiomeScriptableObject[2];
    [SerializeField] Tile emptyCollisionTile;

    [Header("Player Prefab")]
    [SerializeField] GameObject playerPrefab;

    [Header("Player Prefab")]
    [SerializeField] int mapWidth = 200;
    [SerializeField] int mapHeight = 200;

    [Header("Map Preview Image")]
    [SerializeField] RawImage mapPreviewImage;

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
            SetupMap();
            CreateCities();

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
            
            LoadLevel(saveData.biomeMapArray);

            LoadPlayerPos(saveData.playerXpos, saveData.playerYpos); 
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CreateCities();
        }
    }

    

    void CreateCities()
    {
        // Finalmap array values
        //
        // 0 = border / not walkable
        // 1 = grass (random piece)
        // 2 = city area
        // 3 = city street
        //
        //
        //
        //

        finalMapArray = MapModTools.MakeMapAllGrass(finalMapArray);
        finalMapArray = MapModTools.CreateCityAreas(finalMapArray);


        GameManager.CurrentMapArray = finalMapArray;

        //SaveWorld();
        //SpawnPlayer();


        Texture2D mapTexture = new Texture2D(mapWidth, mapHeight);
        mapTexture.filterMode = FilterMode.Point;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                switch (finalMapArray[y, x])
                {
                    case 1:
                        mapTexture.SetPixel(x, y, Color.green); // Grass green
                        break;
                    case 2:
                        mapTexture.SetPixel(x, y, Color.grey); // City area grey
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }
            }
        }

        mapTexture.Apply();
        mapPreviewImage.texture = mapTexture;
    }

    void LoadLevel(int[,] _loadedMapArray)
    {
        finalMapArray = new int[mapWidth, mapHeight];
        finalMapArray = _loadedMapArray;

        ObjectVisibility.Instance.MapObjects = new GameObject[mapWidth, mapHeight];

        GameObject biomeObject = new GameObject("Map");
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
        groundTileMap.size = new Vector3Int(mapWidth, mapHeight, 1);
        borderTileMap.size = new Vector3Int(mapWidth, mapHeight, 1);

        // set grass tiles
        //for (int x = 0; x < mapWidth; x++)
        //{
        //    for (int y = 0; y < mapHeight; y++)
        //    {
        //        Tile currentTile = GetRandomGroundTile(biome);
        //        groundTileMap.SetTile(new Vector3Int(x, y, 0), currentTile);
        //    }
        //}

        // set border collision
        //for (int x = 0; x < biome.biomeWidth; x++)
        //{
        //    for (int y = 0; y < biome.biomeHeight; y++)
        //    {
        //        if (finalMapArray[x, y] == 0)
        //            borderTileMap.SetTile(new Vector3Int(x, y, 0), emptyCollisionTile);
        //    }
        //}


    }

    void SetupMap()
    {
        // Reset visibility array
        ObjectVisibility.Instance.MapObjects = new GameObject[mapHeight, mapWidth];

        // Create biome objects and add scripts for the Unity tilemap system
        GameObject biomeObject = new GameObject("Map");
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
        groundTileMap.size = new Vector3Int(mapWidth, mapHeight, 1);
        borderTileMap.size = new Vector3Int(mapWidth, mapHeight, 1);
        //noiseValues = noise.GetNoiseValues(biome.biomeWidth, biome.biomeHeight);

        seed = Random.Range(0, 5000);
        // Create perlin noise with random seed
        noise = new PerlinNoise(seed, 8, 4f, 1f, 1.3f, 6);
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapWidth, mapHeight, 6f, 7f);
        finalMapArray = new int[mapWidth, mapHeight];

        

        RectTransform rt = mapPreviewImage.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(mapWidth, mapHeight);
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
