using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestCanvas : MonoBehaviour
{
    void Start()
    {
        Invoke("Dead", 1.5f);
    }

    void Update()
    {
        gameObject.transform.position += new Vector3(0, 0.8f * Time.fixedDeltaTime ,0);
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}