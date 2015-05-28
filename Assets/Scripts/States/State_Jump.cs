using UnityEngine;
using System.Collections;

public class State_Jump : State
{
    float time = 0;
    bool double_jump_used = false;

    protected override void Awake() 
	{
        base.Awake();
	}

    public override void UpdateState() 
	{
        if (time < 0.5f) time += Time.deltaTime; //Tiempo mínimo para este estado
        else
        {
            if (controller.IsLanded()) controller.ChangeState<State_Idle>();
            else if (controller.GetButton(button_pad.Square) && controller.HasWeapon()) controller.ChangeState<State_Attack>(); //si ataco
            else
            {
                if (!controller.CanOverhang() || !controller.CanOverhangFoot())
                {
                    float rozamiento = 0.5f;

                    controller.Move(rozamiento);
                }

                if (controller.GetButton(button_pad.Cross) && controller.HasSpecialArmor(Special_Armor_Effect.propulsor) && !double_jump_used)
                {
                    double_jump_used = true;

                    controller.DoubleJump();
                }
            }
        }
	}

    void OnTriggerStay(Collider other)
    {
        if (!paused && !stopTrigger)
        {
            if (other.tag == "Stairs")
            {
                if (controller.GetPad(direcction_pad.Up) >= 0.8f || controller.GetPad(direcction_pad.Down) <= -0.8f) { stopTrigger = true; controller.ChangeState<State_UpStairs>(other.gameObject); }
            }
            else if (other.GetComponent<Overhang>())
            {
                bool direction = false;

                if (other.transform.position.x < transform.position.x && controller.GetDir() == -1) direction = true;
                else if (other.transform.position.x > transform.position.x && controller.GetDir() == 1) direction = true;

                if (direction && Vector3.Distance(transform.position + Vector3.up, other.transform.position) < 0.33f) { stopTrigger = true; controller.ChangeState<State_Overhang>(); }
            }
            else if (other.tag == "Rope" && controller.CanOverhang()) { stopTrigger = true; controller.ChangeState<State_HangRope>(); }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (!paused && !stopTrigger)
        {
            if (!controller.IsLanded() && controller.CanOverhang() && controller.CanOverhangFoot() && collisionInfo.gameObject.layer == 9) 
            {
                bool direction = false;
                int mando_dir = (int)Mathf.Sign((controller.GetPad(direcction_pad.Right) + controller.GetPad(direcction_pad.Left)));
                
                if (collisionInfo.transform.position.x < transform.position.x && mando_dir == -1) direction = true;
                else if (collisionInfo.transform.position.x > transform.position.x && mando_dir == 1) direction = true;

                if (direction) { stopTrigger = true; controller.ChangeState<State_WallSlide>(); }
            }
        }
    }

    public override void OnMachineStateExit(States new_state)
    {
        
    }
}
