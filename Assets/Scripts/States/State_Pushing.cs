using UnityEngine;
using System.Collections;

public class State_Pushing : State 
{
    Rigidbody box;
    float default_mass, dead_time, dead_time_default = 1;

    protected override void Awake() 
	{
        base.Awake();

        controller.GetRigidbody().velocity = Vector3.zero;
	}

    public override void UpdateState() 
	{
        if (!controller.IsLanded()) controller.ChangeState<State_Jump>(); //si estoy cayendo
        else if (controller.GetButton(button_pad.Cross)) controller.Jump(); //si salto
        else if (controller.GetButton(button_pad.Circle)) controller.ChangeState<State_Idle>(); //si dejo de empujar
        else if (box) //si tengo algo que empujar
        {
            if (Vector3.Distance(box.position, controller.transform.position) <= 2)
            {
                int dir = (box.transform.position.x < controller.GetRigidbody().position.x) ? -1 : 1;

                controller.Push(dir);
            }
            else controller.ChangeState<State_Idle>();
        }
        else
        {
            dead_time += Time.fixedDeltaTime;
            if (dead_time >= dead_time_default) controller.ChangeState<State_Idle>();
        }
	}
    
    public override void OnMachineStateEnter(GameObject other)
    {
        base.OnMachineStateEnter(other);

        box = interactuable.GetComponent<Rigidbody>();
        default_mass = box.mass;
        box.mass = 1.75f;

        ConfigurableJoint enlace = box.gameObject.AddComponent<ConfigurableJoint>();
        enlace.connectedBody = controller.GetRigidbody();
        enlace.enableCollision = true;
        enlace.angularZMotion = ConfigurableJointMotion.Locked;
        enlace.zMotion = ConfigurableJointMotion.Locked;
        enlace.axis = Vector3.forward;

        JointDrive conf = new JointDrive();
        conf.mode = JointDriveMode.Position;
        conf.positionSpring = 2;
        conf.maximumForce = enlace.xDrive.maximumForce;
        enlace.xDrive = conf;
    }

    public override void OnMachineStateExit(States new_state)
    {
        machine.ResumeAnimator();
        if (box)
        {
            box.mass = default_mass;
            Destroy(box.GetComponent<ConfigurableJoint>());
        }
    }
}
