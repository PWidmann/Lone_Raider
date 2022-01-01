using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BiomeData", menuName = "ScriptableObjects/Biome", order = 1)]
public class BiomeScriptableObject : ScriptableObject
{
    [SerializeField] public string biomeName;

    [SerializeField] public int biomeWidth;
    [SerializeField] public int biomeHeight;


    [Tooltip("Basic ground tiles drawn before everything else")]
    [SerializeField] public Tile[] groundMapTiles = new Tile[10];
    [Tooltip("Prefabs which build the map border")]
    [SerializeField] public GameObject[] littleBorderObjects;
    [SerializeField] public GameObject[] bigBorderObjects;
    [SerializeField] public GameObject[] walkbyWiggleGrass;
}
