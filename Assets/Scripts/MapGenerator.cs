using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int mapSizeX;
    [SerializeField] int mapSizeY;

    [SerializeField] Tilemap groundTileMap;

    [Header("Map Tiles")]
    public Tile[] groundMapTiles = new Tile[10];

    [Header("Player Preafab")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject walkbyPlantPrefab;
    [SerializeField] GameObject tree1Prefab;
    [SerializeField] Transform plantParent;
    [SerializeField] int walkbyPlantCount;
    [SerializeField] int treeCount;

    private void Start()
    {
        CreateGroundMap();
        SpawnPlayer(mapSizeX / 2, mapSizeY / 2);
    }

    void CreateGroundMap()
    {
        groundTileMap.ClearAllTiles();
        groundTileMap.size = new Vector3Int(mapSizeX, mapSizeY, 1);

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                Tile currentTile = GetGroundTile();
                groundTileMap.SetTile(new Vector3Int(x, y, 0), currentTile);
            }
        }

        for (int i = 0; i < walkbyPlantCount; i++)
        {
            GameObject plant = Instantiate(walkbyPlantPrefab);
            plant.transform.parent = plantParent;

            plant.gameObject.transform.position = new Vector3(Random.Range(0f, (float)mapSizeX), Random.Range(0f, (float)mapSizeY), -1f);
        }

        for (int i = 0; i < treeCount; i++)
        {
            GameObject tree = Instantiate(tree1Prefab);
            tree.transform.parent = plantParent;

            tree.gameObject.transform.position = new Vector3(Random.Range(0f, (float)mapSizeX), Random.Range(0f, (float)mapSizeY), -1f);
        }
    }

    void SpawnPlayer(int xPos, int yPos)
    {
        GameObject player = Instantiate(playerPrefab);
        player.gameObject.transform.position = new Vector3(xPos, yPos, -1);
    }

    Tile GetGroundTile()
    {
        Tile tile;

        int rnd = Random.Range(0, 100);

        if (rnd > 15)
        {
            int rnd2 = Random.Range(0, 4);

            switch (rnd2)
            {
                case 0:
                    tile = groundMapTiles[0];
                    break;
                case 1:
                    tile = groundMapTiles[1];
                    break;
                case 2:
                    tile = groundMapTiles[2];
                    break;
                case 3:
                    tile = groundMapTiles[3];
                    break;
                default:
                    tile = groundMapTiles[0];
                    break;
            }
        }
        else
        {
            // 15 % chance on flower tile
            tile = groundMapTiles[Random.Range(4, groundMapTiles.Length)];
        }


        return tile;
    }
}
