using System.Collections;
using ScriptableObject;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float smoothness;
    [SerializeField] private float seed;
    [SerializeField] private float noiseFrequency;
    [SerializeField] [Range(0, 1)] private float caveFrequency;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private Tilemap groundTilemap;

    // [SerializeField] private GameEvent onFinishGeneration;

    [SerializeField] private GameObject[] playerGameObjects;

    [SerializeField] private WorldGenNoise worldGenNoise;

    // public Dictionary<(int, int), bool> Map = new();
    private readonly Hashtable _surfaceTile = new();

    private static float SumNoise(int x, int y, WorldGenNoise noiseProfile)
    {
        var freq = noiseProfile.startFequency;
        float amp = 1;
        float noiseSum = 0;
        float ampSum = 0;
        for (var i = 0; i < noiseProfile.octave; i++)
        {
            noiseSum += amp * Mathf.PerlinNoise(x * freq, y * freq);
            ampSum += amp;
            amp *= noiseProfile.presistance;
            freq *= noiseProfile.frequencyModifier;
        }

        return noiseSum / ampSum;
    }

    private static float Fit(float input, float inMin, float inMax, float outMin, float outMax) =>
        outMin + (input - inMin) * (outMax - outMin) / (inMax - inMin);

    private void GenerateTerrain()
    {
        groundTilemap.ClearAllTiles();
        for (var x = -width; x < width; x++)
        {
            var yCoord = GetYCoord(x);
            for (var y = -64; y < yCoord; y++)
            {
                var isCave = Mathf.RoundToInt(Mathf.PerlinNoise(x * caveFrequency, y * caveFrequency)) == 1;
                if (isCave || y < -58)
                {
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
                }
            }
            _surfaceTile[x] = yCoord + 1;
        }
        // onFinishGeneration.Raise(this, seed);
        foreach (var player in playerGameObjects)
        {
            player.transform.position = GetSpawnableTile();
        }
    }

    private int GetYCoord(int x)
    {
        var noise = SumNoise(worldGenNoise.offset.x + x, 1, worldGenNoise);
        var noiseInRange =
            Fit(noise, 0, 1, worldGenNoise.noiseRangeMin, worldGenNoise.noiseRangeMax);
        var yCoord = Mathf.FloorToInt(noiseInRange);
        return yCoord;
    }

    public Vector3Int GetSpawnableTile()
    {
        var x = Mathf.RoundToInt(Random.Range(-width * 0.5f, width * 0.5f));
        return new Vector3Int(x, (int)_surfaceTile[x], 0);
    }
    
    private void Start()
    {
        GenerateTerrain();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         GenerateTerrain();
    //     }
    // }
}