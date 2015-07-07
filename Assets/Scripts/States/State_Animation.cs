using UnityEngine;
using System.Collections;

public class State_Animation : State 
{
    int code;

    protected override void Awake() 
	{
        base.Awake();
        controller.GetRigidbody().velocity = Vector3.zero;
	}

    public void SetAnimation(int c)
    {
        Debug.Log("Animacion dentro: "+name+" "+c);

        code = c;

        machine.SetValAnim("AnimationCode", code);
    }

    public void Finished()
    {
        machine.ChangeState<State_Idle>();
    }

    public override void UpdateState()
    {
        
	}

    public override void OnMachineStateExit(States new_state)
    {
        
    }
}
