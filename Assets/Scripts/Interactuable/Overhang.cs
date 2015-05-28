using UnityEngine;
using System.Collections;
using System;

public class Overhang : MonoBehaviour
{
    int distance = 3;
    GameObject icon;

    void Start()
    {
        icon = transform.GetChild(0).gameObject;
    }


    void Update()
    {
        if (Vector3.Distance(GameController.current.player.transform.position, transform.position) < distance)
        {
            icon.SetActive(true);
        }
        else if (icon.activeSelf) icon.SetActive(false);
    }

    
}
