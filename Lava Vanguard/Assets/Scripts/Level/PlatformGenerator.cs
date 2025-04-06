using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public static PlatformGenerator Instance { get; private set; }
    public Transform platformContainer;
    public GameObject platformPrefab;
    public List<PlatformView[]> platforms = new List<PlatformView[]>();

    private int layerIndex = 0;
    private static readonly int COL = 5;
    private static readonly float InitialY = -3;
    private static readonly float InitialX = (COL / 2) * (-5);
    private static readonly float IntervalY = 2;
    private static readonly float IntervalX = 5;

    private void Awake()
    {
        Instance = this;
    }
    public void GenerateOneLayer(bool[] preset)
    {
        var layer = new PlatformView[COL];
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

        var lastLayer = platforms[^1];
        var currentLayer = new PlatformView[COL];
        int count = Random.Range(COL / 2 - COL / 4, COL / 2 + COL / 4 + 1);
        bool[] reach = new bool[COL];
        for (int i = 0; i < lastLayer.Length; i++)
        {
            if (lastLayer[i] != null)
            {
                reach[Mathf.Clamp(i - 1, 0, COL - 1)] = true;
                reach[i] = true;
                reach[Mathf.Clamp(i + 1, 0, COL - 1)] = true;
            }
        }

        if (platforms.Count >= 2)
            for (int i = 0; i < reach.Length; i++) 
            {
                if (platforms[^1][i] != null && platforms[^2][i] != null)
                    reach[i] = false;
            }



        List<int> trueIndices = new List<int>();

        for (int i = 0; i < reach.Length; i++)
        {
            if (reach[i])
            {
                trueIndices.Add(i);
            }
        }

        count = Mathf.Min(count, trueIndices.Count);
        trueIndices = trueIndices.OrderBy(x => Random.value).ToList();

        trueIndices = trueIndices.Take(count).ToList();

        for (int i = 0; i < trueIndices.Count; i++)
        {
            currentLayer[trueIndices[i]] = CreatePlatform(trueIndices[i]);
        }
        platforms.Add(currentLayer);
        layerIndex++;
    }
    private void RemoveOneLayer()
    {
        if (platforms.Count >= 8)
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
        float offsetY = new float[] { -0f, 0f, 0f }[Random.Range(0, 3)];
        Vector2 position = new Vector2(InitialX + column * IntervalX + offsetX, InitialY + layerIndex * IntervalY + offsetY);

        // Instantiate the platform and initialize it
        PlatformView platform = Instantiate(platformPrefab, platformContainer).GetComponent<PlatformView>();
        platform.Init(new Vector2(3, 0.5f), position);
        return platform;
    }

    private float nextGenerateY; // The Y position where the next platform should be generated
    private Camera mainCamera;

    public void Init()
    {
        var layer = new PlatformView[COL];
        var init = Instantiate(platformPrefab, platformContainer).GetComponent<PlatformView>();
        init.Init(new Vector2(3, 0.5f), new Vector2(0, InitialY));
        layer[COL / 2] = init;
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
        int cnt = Tutorial.Instance.tutorial ? 2 : 5;
        for (int i = 0; i < cnt; i++)
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
