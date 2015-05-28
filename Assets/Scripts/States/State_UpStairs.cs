using UnityEngine;
using System.Collections;

public class State_UpStairs : State 
{
    float escalera_impulso = 5f;

    protected override void Awake() 
	{
        base.Awake();

        controller.GetRigidbody().velocity = Vector3.zero;
        controller.GetRigidbody().useGravity = false;

        machine.SetRootMotion(true);
	}

    public override void UpdateState() 
	{
        if (controller.GetButton(button_pad.Cross)) controller.ChangeState<State_Jump>(); //si me suelto
        else
        {
            if (controller.IsLanded() && (controller.GetPad(direcction_pad.Right) >= 0.6f || controller.GetPad(direcction_pad.Left) <= -0.6f)) controller.ChangeState<State_Idle>();
            else controller.UpStairs(escalera_impulso, interactuable);
        }
	}
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stairs") controller.ChangeState<State_Idle>();
    }

    public override void OnMachineStateExit(States new_state)
    {
        machine.ResumeAnimator();
        controller.GetRigidbody().useGravity = true;

        float velocity_x = controller.GetAceleration() * (controller.GetPad(direcction_pad.Right) + controller.GetPad(direcction_pad.Left)) * 200;
        controller.GetRigidbody().AddForce(new Vector3(velocity_x,0,0));

        machine.SetRootMotion(false);

        if (controller.GetPad(direcction_pad.Left) < 0)
        {
            controller.SetDir(-1);
            machine.SetValAnim("Dir", -1);
        }
        else if (controller.GetPad(direcction_pad.Right) > 0)
        {
            controller.SetDir(1);
            machine.SetValAnim("Dir", 1);
        }
    }
}