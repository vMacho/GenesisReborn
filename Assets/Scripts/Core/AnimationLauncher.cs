//AnimationLauncher
/******************** VICTOR MACHO **************/
//Al entrar en contacto con el Player lanzamos una animacion in game
/***********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationLauncher : MonoBehaviour
{
    public int Code;
    public List<Controller> Objects = new List<Controller>();
    public List<GameObject> Activate = new List<GameObject>();

    public void SetAnimation()
    {
        foreach (Controller go in Objects)
        {
            if (go != null) go.SendMessage("AnimationPlay", Code, SendMessageOptions.DontRequireReceiver);
        }

        foreach (GameObject go in Activate)
        {
            if (go != null) go.SetActive(true);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SetAnimation();
        }
    }
}