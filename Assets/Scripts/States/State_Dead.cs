using UnityEngine;
using System.Collections;

public class State_Dead : State 
{
    protected override void Awake() 
	{
        base.Awake();
        
        Debug.Log("DEAD "+gameObject.name);
	}

    public override void UpdateState() 
	{
        
	}


    public override void OnMachineStateExit(States new_state)
    {
        
    }
}
