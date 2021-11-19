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

    private void Start()
    {
        worldPlane.transform.localScale = new Vector3(worldSize, 1, worldSize);

        int placed = 0;
        Random.InitState(randValue);
        for (int i = 0; i < treeCount; ++i)
        {
            ++placed;
            GameObject obj;
            int val = Random.Range(0, 3);
            obj = Instantiate(treePrefabs[val]);
            obj.transform.position = new Vector3(Random.Range(-worldSize / 2, worldSize / 2), 0, Random.Range(-worldSize / 2, worldSize / 2));
            obj.name = i.ToString();
        }
    }
}
