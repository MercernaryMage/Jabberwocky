using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int randValue = 1;
    public float worldSize = 100;
    public int treeCount = 50;

    public GameObject worldPlane;

    public List<GameObject> treePrefabs;
    public GameObject socketPrefab;
    public List<GameObject> POIs;

    float treeRadius = 6;
    float socketRadius = 20;
    float POIRadius = 30;

    public GameObject constantPrefab;

    List<GameObject> objectsToBeReset = new List<GameObject>();

    int worldFeatureIndex = 0;
    public List<GameObject> worldFeatures;

    int screenshotCount = 0;
    bool doingScreenShots = false;

    bool PlacementIsValid(List<Vector3> placedObjects, Vector3 currentPlacement, float radius1, float radius2)
    {
        foreach (Vector3 vt in placedObjects)
        {
            if (Vector3.Distance(vt, currentPlacement) <= radius1 + radius2)
            {
                return false;
            }
        }
        return true;
    }

    bool PlacementIsValidForGeo(Vector3 currentPlacement, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(currentPlacement, radius);
        for (int j = 0; j < colliders.Length; ++j)
        {
            if (colliders[j].tag == "Floor")
            {
                continue;
            }
            if (colliders[j].tag == "Player")
            {
                continue;
            }
            return false;
        }
        return true;
    }

    private void Start()
    {
        worldPlane.transform.localScale = new Vector3(worldSize, 1, worldSize);

        GenerateWorld();
    }

    void GenerateWorld()
    {
        List<Vector3> validTrees = new List<Vector3>();
        List<Vector3> validSockets = new List<Vector3>();
        List<Vector3> validPOI = new List<Vector3>();

        for (int i = 0; i < treeCount; ++i)
        {
            float placementRangePositive = worldSize / 2 - treeRadius;
            float placementRangeNegative = -worldSize / 2 + treeRadius;

            Vector3 currentPlacement = new Vector3(Random.Range(placementRangeNegative, placementRangePositive), 0, Random.Range(placementRangeNegative, placementRangePositive));

            if (PlacementIsValid(validTrees, currentPlacement, treeRadius, treeRadius))
            {
                validTrees.Add(currentPlacement);
            }
        }


        for (int i = 0; i < 100 && validSockets.Count < 3; ++i)
        {
            float placementRangePositive = worldSize / 2 - socketRadius;
            float placementRangeNegative = -worldSize / 2 + socketRadius;

            Vector3 currentPlacement = new Vector3(Random.Range(placementRangeNegative, placementRangePositive), 0, Random.Range(placementRangeNegative, placementRangePositive));

            if (!PlacementIsValid(validSockets, currentPlacement, socketRadius, socketRadius))
            {
                continue;

            }
            if (!PlacementIsValidForGeo(currentPlacement, socketRadius))
            {
                continue;
            }

            validSockets.Add(currentPlacement);
        }

        for (int i = 0; i < 100 && validPOI.Count < POIs.Count; ++i)
        {
            float placementRangePositive = worldSize / 2 - POIRadius;
            float placementRangeNegative = -worldSize / 2 + POIRadius;

            Vector3 currentPlacement = new Vector3(Random.Range(placementRangeNegative, placementRangePositive), 0, Random.Range(placementRangeNegative, placementRangePositive));

            if (!PlacementIsValid(validPOI, currentPlacement, POIRadius, POIRadius))
            {
                continue;

            }
            if (!PlacementIsValid(validSockets, currentPlacement, POIRadius, socketRadius))
            {
                continue;
            }
            if (!PlacementIsValidForGeo(currentPlacement, POIRadius))
            {
                continue;
            }

            validPOI.Add(currentPlacement);
        }

        //DELETE INVALID TREES
        for (int i = 0; i < validTrees.Count; ++i)
        {
            if (!PlacementIsValid(validSockets, validTrees[i], socketRadius + 10, treeRadius) ||
                !PlacementIsValid(validPOI, validTrees[i], POIRadius + 10, treeRadius))
            {
                validTrees.RemoveAt(i);
                --i;
            }
        }

        //CHECK FOR GEO
        for (int i = 0; i < validTrees.Count; ++i)
        {
            Collider[] colliders = Physics.OverlapSphere(validTrees[i], treeRadius);
            for (int j = 0; j < colliders.Length; ++j)
            {
                if (colliders[j].tag == "Floor")
                {
                    continue;
                }
                if (colliders[j].tag == "Player")
                {
                    continue;
                }

                validTrees.RemoveAt(i);
                --i;
                break;
            }
        }

        for (int i = 0; i < validTrees.Count; ++i)
        {
            GameObject obj;
            int val = Random.Range(0, 3);
            obj = Instantiate(treePrefabs[val]);
            obj.transform.position = validTrees[i] + worldPlane.transform.position;
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
            obj.name = i.ToString();
            objectsToBeReset.Add(obj);
        }

        for (int i = 0; i < validSockets.Count; ++i)
        {
            GameObject obj;
            obj = Instantiate(socketPrefab);
            obj.transform.position = validSockets[i] + new Vector3(-socketRadius / 2, 10.27f, -socketRadius / 2) + worldPlane.transform.position;
            obj.name = $"socket {i}";
            objectsToBeReset.Add(obj);
        }

        for (int i = 0; i < validPOI.Count; ++i)
        {
            GameObject obj;
            obj = Instantiate(POIs[i]);
            obj.transform.position = validPOI[i] + new Vector3(0, 9.35f, 0) + worldPlane.transform.position;
            obj.name = $"poi {i}";
            objectsToBeReset.Add(obj);
        }
    }

    void DestoryWorld()
    {
        foreach (GameObject obj in objectsToBeReset)
        {
            DestroyImmediate(obj);
        }
        objectsToBeReset.Clear();
    }

    void UpdateWorldFeatures()
    {
        for (int i = 0; i < worldFeatures.Count; ++i)
        {
            worldFeatures[i].SetActive(i == worldFeatureIndex);
        }
    }

    void AdvanceGeneration()
    {
        ++worldFeatureIndex;
        worldFeatureIndex = worldFeatureIndex % worldFeatures.Count;
        UpdateWorldFeatures();
        DestoryWorld();
        GenerateWorld();
    }

    public void SetWorld(int i)
    {
        worldFeatureIndex = i % worldFeatures.Count;
        Random.InitState(i);
        UpdateWorldFeatures();
        DestoryWorld();
        GenerateWorld();
    }

    private void Update()
    {
        if (doingScreenShots)
        {
            Random.InitState(screenshotCount + 1);
            AdvanceGeneration();
            ScreenCapture.CaptureScreenshot($"Screenshots/test{screenshotCount + 1}.png");
            ++screenshotCount;
            if (screenshotCount == 100)
            {
                doingScreenShots = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DestoryWorld();
            GenerateWorld();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AdvanceGeneration();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            worldFeatureIndex = 0;
            screenshotCount = 0;
            doingScreenShots = true;
        }
    }
}
