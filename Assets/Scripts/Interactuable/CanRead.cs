using UnityEngine;
using System.Collections;
using System;

public class CanRead : MonoBehaviour
{
    GameObject aviso;
    public string text;

    void Start()
    {        
        aviso = transform.Find("Canvas").gameObject;
    }

    void OnTriggerStay(Collider other)
    {
        Player c = other.gameObject.GetComponent<Player>();
        if (c != null)
        {
            if (c.GetState() == States.State_Idle || c.GetState() == States.State_Walk) aviso.SetActive(true);
            else aviso.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() && aviso.activeSelf) aviso.SetActive(false);
    }
}
