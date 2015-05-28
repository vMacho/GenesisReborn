using UnityEngine;
using System.Collections;
using System;

public class CanPush : MonoBehaviour
{
    public float mass = 200;
    Rigidbody body;
    Vector3 prev_velocity = Vector3.zero;
    GameObject aviso;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        if (body == null) body = gameObject.AddComponent<Rigidbody>();

        body.mass = mass;

        //body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        
        aviso = transform.Find("Canvas").gameObject;
    }

    void OnPauseGame()
    {
        prev_velocity = body.velocity;
        body.isKinematic = true;
        body.Sleep();
    }

    void OnResumeGame()
    {
        body.velocity = prev_velocity;
        body.isKinematic = false;
        body.WakeUp();
    }
    
    void OnCollisionStay(Collision collisionInfo)
    {
        Player c = collisionInfo.gameObject.GetComponent<Player>();
        if (c)
        {
            if (c.GetState() != States.State_Pushing && c.CanOverhang()) aviso.SetActive(true);
            else aviso.SetActive(false);
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.GetComponent<Player>() && aviso.activeSelf) aviso.SetActive(false);
    }
}
