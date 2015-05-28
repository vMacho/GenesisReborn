using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour
{
    List<PlatformNode> path = new List<PlatformNode>();
    public Color rayColor = Color.white;

    public List<PlatformNode> GetPath() { return path; }

    void Awake() 
    {
        path = new List<PlatformNode>();
        path.AddRange(transform.GetComponentsInChildren<PlatformNode>());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;

        path = new List<PlatformNode>();
        path.AddRange(transform.GetComponentsInChildren<PlatformNode>());

        for (int i = 0; i < path.Count; ++i)
        {
            Vector3 pos = path[i].transform.position;
            Gizmos.DrawWireSphere(pos, 1);
            if (i > 0)
            {
                Vector3 prev = path[i - 1].transform.position;
                Gizmos.DrawLine(prev, pos);
            }

        }
    }
}
