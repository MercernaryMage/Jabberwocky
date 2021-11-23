using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;

    void Start()
    {
        for (int i =0; i < 500; ++i)
        {
            GameObject obj = Instantiate(cubePrefab);
            obj.transform.SetParent(transform);

            float x = Random.Range(-.5f, .5f);
            float z = Random.Range(-.5f, .5f);

            obj.transform.localPosition = new Vector3(x, 0, z);
        }
    }
}
