using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LeverTrigger : ActionController
{
    public List<ActionReceiver> receiver = new List<ActionReceiver>();

    protected override void Awake()
    {
        base.Awake();

        action = ActionType.activate_lever;
    }
    
    public override void PlayAction() 
    {
        base.PlayAction();
        
        foreach(ActionReceiver c in receiver) c.PlayAction();
    }
}
