using UnityEngine;
using System.Collections;

public class ActionReceiver : MonoBehaviour
{
    void Awake()
    {
        if (GetComponent<Animator>()) GetComponent<Animator>().speed = 0;
    }

    public void PlayAction()
    {
        if (GetComponent<Animator>()) GetComponent<Animator>().speed = 1;

    }
}
