using UnityEngine;
using System.Collections;

public class State_WallSlide : State 
{
    float gravity_mod = -1.5f;
    float time = 1.5f;

    protected override void Awake() 
	{
        base.Awake();
        controller.GetRigidbody().velocity = Vector3.zero;
        controller.GetRigidbody().useGravity = false;
	}

    public override void UpdateState() 
	{
        if (controller.IsLanded()) controller.ChangeState<State_Idle>();
        else if (!controller.CanOverhang() || !controller.CanOverhangFoot() || time <= 0) controller.ChangeState<State_Jump>(); //si no se puede seguir agarrando o no tiene tiempo que puede deslizarse
        else
        {
            time -= Time.deltaTime;

            controller.GetRigidbody().velocity = new Vector3(0, gravity_mod, 0);

            if (controller.GetButton(button_pad.Cross))
            {
                float dir = controller.GetPad(direcction_pad.Right) + controller.GetPad(direcction_pad.Left);

                if ((controller.GetDir() == 1 && dir <= -0.5f) || (controller.GetDir() == -1 && dir >= 0.5f)) controller.Wallflip();
            }
        }
	}

    public override void OnMachineStateExit(States new_state)
    {
        controller.GetRigidbody().useGravity = true;
    }
}
