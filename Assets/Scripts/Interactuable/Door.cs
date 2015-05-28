using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : ActionController
{
    protected override void Awake()
    {
        base.Awake();

        action = ActionType.open_door_corrediza;

        GetComponent<Animator>().speed = 0;
    }
    
    public override void PlayAction() 
    {
        base.PlayAction();

        GetComponent<Animator>().speed = 1;
    }
}
