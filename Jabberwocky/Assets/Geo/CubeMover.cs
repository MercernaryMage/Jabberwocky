using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMover : MonoBehaviour
{
    float time = 0;
    void Start()
    {
        time = Random.Range(0, 10);
    }

    void Update()
    {
        time += Time.deltaTime;
        float yValue = Mathf.Cos(time);
        transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
    }
}
