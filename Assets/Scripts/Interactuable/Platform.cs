using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    public List<PlatformNode> path = new List<PlatformNode>();
    public Path pathfollow;
    int actualNode;
    float waitTime = 0, actual_waitTime = 0;


    public float Speed;

    void Awake()
    {
        path = pathfollow.GetPath();
        actualNode = 1;
    }

    void Update()
    {
        if (waitTime > 0 && actual_waitTime < waitTime) actual_waitTime += Time.fixedDeltaTime;
        else
        {
            if (path.Count > 0)
            {
                transform.parent.position = Vector3.Lerp(transform.parent.position, path[actualNode].transform.position, Speed * Time.fixedDeltaTime);

                if (Mathf.Abs(transform.parent.position.magnitude - path[actualNode].transform.position.magnitude) < 1)
                {
                    waitTime = path[actualNode].waitTime;
                    actual_waitTime = 0;
                    actualNode = (path.Count > actualNode + 1) ? actualNode + 1 : 0;
                }
            }
            else path = pathfollow.GetPath();
        }
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        other.transform.parent = gameObject.transform;
    }

    void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
