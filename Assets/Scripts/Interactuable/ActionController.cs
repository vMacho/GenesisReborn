using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour
{
    protected GameObject aviso;
    protected bool _activated;
    protected ActionType action;

    public bool IsActivated() { return _activated; }
    public ActionType GetAction() { return action; }

    protected virtual void Awake()
    {
        aviso = transform.Find("Canvas").gameObject;

        if (GetComponent<Animator>()) GetComponent<Animator>().speed = 0;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!_activated)
        {
            Player c = other.gameObject.GetComponent<Player>();

            if (c != null)
            {
                if (c.GetState() == States.State_Idle || c.GetState() == States.State_Walk) aviso.SetActive(true);
                else aviso.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() && aviso.activeSelf) aviso.SetActive(false);
    }

    public virtual void PlayAction()
    {
        if (GetComponent<Animator>()) GetComponent<Animator>().speed = 1;

        _activated = true;
        aviso.SetActive(false);
    }
}

public enum ActionType
{
    none,
    open_chest,
    activate_lever,
    open_door_corrediza
}
