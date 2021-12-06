using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapSpliiter : MonoBehaviour
{
    public GameObject root;
    public TextMeshProUGUI field1;
    public TextMeshProUGUI field2;

    public WorldGenerator gen1;
    public WorldGenerator gen2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (root.activeInHierarchy)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    void TurnOn()
    {
        Cursor.visible = true;
        root.SetActive(true);
    }

    void TurnOff()
    {
        Cursor.visible = false;
        root.SetActive(false);
        char[] arr = field1.text.ToCharArray();

        arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                          || char.IsWhiteSpace(c))));
        string str1 = new string(arr);

        arr = field2.text.ToCharArray();
        arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                          || char.IsWhiteSpace(c))));
        string str2 = new string(arr);

        int value1 = System.Convert.ToInt32(str1);
        int value2 = System.Convert.ToInt32(str2);

        gen1.SetWorld(value1);
        gen2.SetWorld(value2);
    }
}
