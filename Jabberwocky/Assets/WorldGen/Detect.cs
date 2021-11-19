using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detect : MonoBehaviour
{
    public bool dead = false;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3);

        foreach (Collider c in colliders)
        {
            if (c.tag == "Floor")
            {
                continue;
            }
            if (c.tag == "Player")
            {
                continue;
            }
            if (c.gameObject == gameObject)
            {
                continue;
            }
            Detect d = c.gameObject.GetComponent<Detect>();
            if (d && d.dead == true)
            {
                continue;
            }
            Debug.Log(c.gameObject.name);
            dead = true;
            Destroy(gameObject);
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }
}
