using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance {  get; private set; }
    public GameObject groundPrefab;
    public GameObject lavaPrefab;
    public GameObject wallPrefab;
    public Transform groundContainer;
    public Lava lava;
    public List<GameObject> grounds = new List<GameObject>();


    private static readonly int[] XStartPos = new int[2] { -8, -12 };
    private static readonly int XInterval = 8;

    public float cameraDistance = 5.0f;
    public float yGap = 1.5f;
    private float lastY;
    private int groundType = 0;
    private int typeCount = 2;

    [Header("Platform generator parameters")]
    public int minPlatformsPerRow = 2;
    public int maxPlatformsPerRow = 4;

    public float minPlatformLength = 2f;
    public float maxPlatformLength = 5f;
    public float maxHorizontalJump = 2.5f;
    public float xMin = -15f, xMax = 15f;
    private List<PlatformData> previousRowPlatforms = new List<PlatformData>();
    private List<int> PreviousReachable = new List<int>();

    public GameObject WavePlatform = null;
    public bool GenWavePlatform = false;
    private bool generating = false;
    private struct PlatformData
    {
        public float centerX;
        public float halfLength;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Instantiate(wallPrefab, new Vector3(-17f, 0f, 0f), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(17f, 0f, 0f), Quaternion.identity);
        GameObject upWall=Instantiate(wallPrefab, new Vector3(0f, 7.5f, 0f), Quaternion.identity);
        upWall.transform.localScale = new Vector3(32f, 4f, 1f);

        lava = Instantiate(lavaPrefab, new Vector3(0f, -10f, 0f), Quaternion.identity).GetComponent<Lava>();
        GameObject initalGround = Instantiate(groundPrefab, new Vector3(0, -5, 0), Quaternion.identity);
        initalGround.transform.localScale = new Vector3(30f, 5f, 1f);
        lastY = -2.5f;

        if (!Tutorial.Instance.tutorial)
            StartGenerate();
    }
    public void StartGenerate()
    {
        generating = true;
    }
    private void Update()
    {
        if (GenWavePlatform)
        {
            GenWavePlatform = false;
            GenerateWavePlatform();
        }
        else if (generating && Camera.main.transform.position.y + cameraDistance > lastY)
        {
            // GenerateGround();
            
            RandomGround();
        }
    }

    private void GenerateGround()
    {
        lastY += yGap;
        if(groundType==0){
            for (int i = 0; i < 3; i++)
            {
                var ground = Instantiate(groundPrefab, new Vector3(XStartPos[groundType] + i * XInterval, lastY, 0f), Quaternion.identity, groundContainer);
                grounds.Add(ground);
            }
        }
        else if(groundType==1){
            for (int i = 0; i < 4; i++)
            {
                var ground = Instantiate(groundPrefab, new Vector3(XStartPos[groundType] + i * XInterval, lastY, 0f), Quaternion.identity, groundContainer);
                grounds.Add(ground);
            }
        }

        groundType = (groundType + 1) % typeCount;
    }

    private void RandomGround() {

        lastY += yGap;
        List<PlatformData> current = new List<PlatformData>();
        int numPlatforms = Random.Range(minPlatformsPerRow, maxPlatformsPerRow + 1);

        // guarantee at least one platform is reachable
        if (previousRowPlatforms.Count > 0) {
            var prev = previousRowPlatforms[PreviousReachable[Random.Range(0, PreviousReachable.Count)]];
            // Within the jump range
            float offset = Random.Range(-maxHorizontalJump, maxHorizontalJump);
            
            float platformLength = Random.Range(minPlatformLength, maxPlatformLength);
            float halfLength = platformLength / 2;
            // The new platform should be within (xMin, xMax)
            float centerX = offset > 0 ? (prev.centerX + prev.halfLength + offset + halfLength) : (prev.centerX - prev.halfLength + offset - halfLength);
            centerX = Mathf.Clamp(centerX, xMin + halfLength, xMax - halfLength);

            var platform = Instantiate(groundPrefab, new Vector3(centerX, lastY, 0), Quaternion.identity, groundContainer);

            Vector3 scale = platform.transform.localScale;
            scale.x = platformLength;
            platform.transform.localScale = scale;

            current.Add(new PlatformData {
                centerX = centerX,
                halfLength = halfLength,
            });

            numPlatforms -= 1;

            grounds.Add(platform);
        }

        float rowWidth = xMax - xMin;
        float segmentWidth = rowWidth / numPlatforms;
        
        for (int i = 0; i < numPlatforms; i++)
        {
            // Randomly assign a platform length.
            float platformLength = Random.Range(minPlatformLength, maxPlatformLength);
            float halfLength = platformLength / 2f;
            
            // Determine the boundaries of this segment.
            float segStart = xMin + i * segmentWidth;
            float segEnd = xMin + (i + 1) * segmentWidth;

            float centerX = Random.Range(segStart + halfLength, segEnd - halfLength);
            
            var platform = Instantiate(groundPrefab, new Vector3(centerX, lastY, 0), Quaternion.identity, groundContainer);

            Vector3 scale = platform.transform.localScale;
            scale.x = platformLength;
            platform.transform.localScale = scale;
            
            current.Add(new PlatformData {
                centerX = centerX,
                halfLength = halfLength,
            });

            grounds.Add(platform);
        }

        GetReachablePlatforms(ref current);

        previousRowPlatforms = current;
    }

    private void GetReachablePlatforms(ref List<PlatformData> current) {
        var currentReachable = new List<int>();
        if (PreviousReachable.Count == 0) {
            for (int i = 0; i < current.Count; i++) {
                PreviousReachable.Add(i);
            }
            return;
        }

        for (int i = 0; i < current.Count; i++) {
            for (int j = 0; j < PreviousReachable.Count; j++) {
                //Debug.Log(PreviousReachable[j]);
                var c = current[i];
                var p = previousRowPlatforms[PreviousReachable[j]];
                bool reachable = false;
                if (c.centerX < p.centerX) {
                    float cRight = c.centerX + c.halfLength;
                    float pLeft = p.centerX - p.halfLength;
                    if (cRight >= pLeft || pLeft - cRight <= maxHorizontalJump) {
                        reachable = true;
                    }
                } else {
                    float cLeft = c.centerX - c.halfLength;
                    float pRight = p.centerX + p.halfLength;
                    if (pRight >= cLeft || cLeft - pRight <= maxHorizontalJump) {
                        reachable = true;
                    }
                }

                if (reachable) {
                    currentReachable.Add(i);
                    break;
                }
            }
        }

        PreviousReachable = currentReachable;
    }

    private void GenerateWavePlatform()
    {
        lastY += yGap;
        previousRowPlatforms.Clear();
        PreviousReachable.Clear();
        var wavePlatform = Instantiate(groundPrefab, new Vector3(0, lastY, 0), Quaternion.identity, groundContainer);
        wavePlatform.name = "wave_platform";
        Vector3 scale = wavePlatform.transform.localScale;
        scale.x = 30f;
        scale.y *= 1.5f;
        wavePlatform.transform.localScale = scale;
        LevelManager.Instance.BreakTime = false;
        WavePlatform = wavePlatform;
    }
}
