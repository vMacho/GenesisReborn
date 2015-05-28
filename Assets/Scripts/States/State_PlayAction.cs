using UnityEngine;
using System.Collections;

public class State_PlayAction : State 
{
    ActionController objAction;
    bool finished = false;

    protected override void Awake()
    {
        base.Awake();

    }

    public override void UpdateState() 
	{
        if (finished) controller.ChangeState<State_Idle>();
	}

    public void PlayAction() 
    {
        if (objAction != null) objAction.PlayAction();
    }

    public void ActionFinished() { finished = true; machine.SetValAnim("TypeAction", 0);  }

    public override void OnMachineStateEnter(GameObject other)
    {
        base.OnMachineStateEnter(other);

        objAction = interactuable.GetComponent<ActionController>();

        machine.SetValAnim("TypeAction", (int)objAction.GetAction());
    }

    public override void OnMachineStateExit(States new_state)
    {
        
    }
}
