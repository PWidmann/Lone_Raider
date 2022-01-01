using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMap : MonoBehaviour
{
    public static WorldMap Instance;

    [SerializeField] private RawImage mapImage;
    [SerializeField] private GameObject playerArrow;

    [SerializeField] private Transform bottomleft;
    [SerializeField] private Transform bottomright;
    [SerializeField] private Transform topleft;
    [SerializeField] private Transform topright;

    private int mapWidth = 200;
    private int mapHeight = 200;

    [HideInInspector]
    public Texture2D mapTexture;
    private Texture2D tempTexture;

    private float refreshTimer = 0;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        mapTexture = new Texture2D(mapWidth, mapHeight);
        mapTexture.filterMode = FilterMode.Point;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                mapTexture.SetPixel(x, y, Color.black);
            }
        }
        mapTexture.Apply();
        mapImage.texture = mapTexture;
    }

    void Update()
    {
        refreshTimer += Time.deltaTime;

        if (refreshTimer > 1f)
        {
            refreshTimer = 0;

            mapTexture.Apply();
            mapImage.texture = mapTexture;
        }


        // Player position on map
        Vector2 imagePosBottomLeft = bottomleft.position;
        float width = bottomright.position.x - bottomleft.position.x;
        float height = topleft.position.y - bottomleft.position.y;
        float percentX = GameManager.PlayerWorldPos.x / 200;
        float percentY = GameManager.PlayerWorldPos.y / 200;

        playerArrow.transform.position = imagePosBottomLeft + new Vector2(width * percentX, height * percentY);
    }

    public void SetPixel(int _x, int _y, Color _color)
    {
        tempTexture = mapTexture;

        tempTexture.SetPixel(_x, _y, _color);
    }
}
