using UnityEngine;
using System.Collections;

public class HideFace : MonoBehaviour
{
    Renderer r;

    void Start() 
    {
        r = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Color c = r.material.color;
            r.material.color = new Color(c.r, c.g, c.b, 0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Color c = r.material.color;
            r.material.color = new Color(c.r, c.g, c.b, 1);
        }
    }
}
