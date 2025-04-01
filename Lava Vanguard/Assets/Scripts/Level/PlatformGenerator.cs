using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public static PlatformGenerator Instance { get; private set; }
    public Transform platformContainer;
    public GameObject platformPrefab;
    public List<PlatformView[]> platforms = new List<PlatformView[]>();

    private int layerIndex = 0;

    private static readonly float InitialY = -3;
    private static readonly float InitialX = -5;
    private static readonly float IntervalY = 2;
    private static readonly float IntervalX = 5;

    private int lastR = -1;         // Stores the last generated type (1 or 2)
    private int secondLastR = -1;   // Stores the second last generated type
    private void Awake()
    {
        Instance = this;
    }
    public void GenerateOneLayer(bool[] preset)
    {
        var layer = new PlatformView[3];
        for (int i = 0; i < preset.Length; i++)
        {
            if (preset[i])
            {
                layer[i] = CreatePlatform(i);
            }
        }
        platforms.Add(layer);
        layerIndex++;
    }
    public void GenerateOneLayer()
    {
        

        // 65% chance to generate one platform, 35% chance to generate two platforms
        float rand = Random.Range(0f, 1f);
        int r = (rand < 0.65f) ? 1 : 2;

        // Ensure the same type (r) does not appear three times in a row
        while (r == lastR && r == secondLastR)
        {
            rand = Random.Range(0f, 1f);
            r = (rand < 0.65f) ? 1 : 2;
        }
        secondLastR = lastR;
        lastR = r;

        var layer = new PlatformView[3] { null, null, null };
        // Get the count of consecutive platforms in each column
        int[] columnCounts = GetRecentColumnCounts();
        List<int> validPositions = new List<int> { 0, 1, 2 };

        // Remove positions that have had platforms for the last 3 layers
        for (int i = 0; i < 3; i++)
        {
            if (columnCounts[i] >= 3)
            {
                validPositions.Remove(i);
            }
        }

        // Ensure at least one valid position exists
        if (validPositions.Count == 0) return;
        else
        {
            var lastLayer = platforms[^1];

            if (r == 1)
            {
                if (lastLayer[1] != null)
                {
                    int p = validPositions[Random.Range(0, validPositions.Count)];
                    layer[p] = CreatePlatform(p);
                }
                else if (lastLayer[0] != null || lastLayer[2] != null)
                {
                    if (validPositions.Contains(1))
                    {
                        layer[1] = CreatePlatform(1);
                    }
                }
            }
            else
            {
                int p = validPositions[Random.Range(0, validPositions.Count)];
                foreach (int i in validPositions)
                {
                    if (i != p)
                    {
                        layer[i] = CreatePlatform(i);
                    }
                }
            }
        }

        platforms.Add(layer);
        layerIndex++;
    }
    private void RemoveOneLayer()
    {
        if (platforms.Count >= 6)
        {
            var layer0 = platforms[0];
            platforms.RemoveAt(0);
            for (int i = 0; i < layer0.Length; i++)
            {
                if (layer0[i] != null) 
                {
                    Destroy(layer0[i].gameObject);
                }
            }
        }
    }

    public PlatformView CreatePlatform(int column)
    {
        // Small random offset of -0.5, 0, or 0.5 for variation
        float offsetX = new float[] { -0f, 0f, 0f }[Random.Range(0, 3)];
        Vector2 position = new Vector2(InitialX + column * IntervalX + offsetX, InitialY + layerIndex * IntervalY);

        // Instantiate the platform and initialize it
        PlatformView platform = Instantiate(platformPrefab, platformContainer).GetComponent<PlatformView>();
        platform.Init(new Vector2(3, 0.5f), position);
        return platform;
    }

    private int[] GetRecentColumnCounts()
    {
        int[] counts = new int[3] { 0, 0, 0 };

        // Check last 3 layers (or fewer if there aren't 3 yet)
        int layersToCheck = Mathf.Min(3, platforms.Count);
        for (int i = 0; i < layersToCheck; i++)
        {
            var layer = platforms[platforms.Count - 1 - i];
            for (int j = 0; j < 3; j++)
            {
                if (layer[j] != null)
                {
                    counts[j]++;
                }
            }
        }

        return counts;
    }
    private float nextGenerateY; // The Y position where the next platform should be generated
    private Camera mainCamera;

    public void Init()
    {
        var layer = new PlatformView[3] { null, null, null };
        var init = Instantiate(platformPrefab, platformContainer).GetComponent<PlatformView>();
        init.Init(new Vector2(3, 0.5f), new Vector2(0, InitialY));
        layer[1] = init;
        platforms.Add(layer);
        layerIndex++;
    }
    private void Start()
    {
        mainCamera = Camera.main;
        //Init();
        nextGenerateY = mainCamera.transform.position.y + IntervalY;
    }
    public void StartGenerating()
    {
        // Generate the first 4 layers at the beginning
        for (int i = 0; i < 4; i++)
        {
            GenerateOneLayer();
        }
    }
    private void Update()
    {
        // Check if the camera has moved past the threshold
        if (mainCamera.transform.position.y >= nextGenerateY)
        {
            GenerateOneLayer();
            nextGenerateY += IntervalY; // Update the threshold for the next layer
            RemoveOneLayer();
        }
    }
}
