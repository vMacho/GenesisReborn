using UnityEngine;
using System.Collections;

public class State_Overhang : State 
{
    bool up = false, down = false;

    protected override void Awake() 
	{
        base.Awake();

        controller.GetRigidbody().velocity = Vector3.zero;
        controller.GetRigidbody().useGravity = false;
	}

    public override void UpdateState() 
	{
        if (!down && !up)
        {
            if (controller.GetButton(button_pad.Cross) || controller.GetPad(direcction_pad.Up) > 0.6f)
            {
                up = true;

                float dir = controller.GetPad(direcction_pad.Right) + controller.GetPad(direcction_pad.Left);

                if ((controller.GetDir() == 1 && dir <= -0.5f) || (controller.GetDir() == -1 && dir >= 0.5f))
                {
                    controller.GetRigidbody().useGravity = true;
                    controller.Wallflip(); //saltamos en direccion contraria
                    Invoke("DownFinished", 0.25f);
                }
                else //subimos el muro
                {
                    machine.SetRootMotion(true);
                    machine.SetTrigger("Up");
                }
            }
            else if (controller.GetPad(direcction_pad.Down) <= -0.6f) //si me suelto del saliente
            {
                down = true;
                controller.GetRigidbody().useGravity = true;

                Invoke("DownFinished", 0.25f);
            }
        }
	}

    public void UpFinished()
    {
        controller.ChangeState<State_Idle>();
    }

    public void DownFinished()
    {
        controller.ChangeState<State_Jump>();
    }
    
    public override void OnMachineStateExit(States new_state)
    {
        machine.ResetTrigger("Up");
        controller.GetRigidbody().useGravity = true;
        machine.SetRootMotion(false);
    }
}
