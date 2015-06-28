using UnityEngine;
using System.Collections;

public class State_Idle : State 
{
    float bored_time = 30, bored_time_actual = 0, time_to_canPush;
    bool _bored;
    GameObject lastCollision;

    protected override void Awake() 
	{
        base.Awake();
        controller.GetRigidbody().velocity = Vector3.zero;

        machine.ResetTrigger("Bored");
	}

    public override void UpdateState()
    {
        if (!controller.IsLanded()) controller.ChangeState<State_Jump>(); //si estoy cayendo
        else if (controller.GetButton(button_pad.Cross)) controller.Jump(); //si salto
        else if (controller.GetButton(button_pad.Triangle)) //si abro el inventario
        {
            controller.ChangeState<State_SearchingBag>();
            GameController.current.OpenInventory(controller.GetInvetory(), controller, InventoryMode.self);
        }
        else if (controller.GetButton(button_pad.R3)) //si abro el menú de Quests
        {
            controller.ChangeState<State_SearchingBag>();
            GameController.current.OpenQuests();
        }
        else if (controller.GetButton(button_pad.Square) && controller.HasWeapon()) controller.ChangeState<State_Attack>(); //si ataco
        else if (!controller.IsBlendDown() && (controller.GetPad(direcction_pad.Left) <= -0.6f || controller.GetPad(direcction_pad.Right) >= 0.6f)) controller.ChangeState<State_Walk>(); //si ando
        else if (controller.GetPad(direcction_pad.Down) <= -0.5f) //si me agacho
        {
            if ( !controller.IsBlendDown())  controller.BlendDown(); //si no estaba agachado

            if (controller.GetButton(button_pad.R2) && controller.IsBlendDown()) controller.ChangeState<State_Walk>(); //si ruedo
        }
        else //Update Idle
        {
            if( controller.IsBlendDown() ) controller.Raise(); //si ya no estoy agachado

            if (gameObject.tag == "Player") //si soy el player
            {
                bored_time_actual += Time.fixedDeltaTime;

                if (!_bored && bored_time_actual >= bored_time)
                {
                    _bored = true;
                    controller.Say("Mueve capullo, no tengo todo el día!!", 5);

                    Camera.main.GetComponent<Player_Camera>().PlayShake();

                    machine.SetTrigger("Bored");
                }
            }
        }

        if (!stopTrigger && lastCollision != null)
        {
            if (Vector3.Distance(lastCollision.transform.position, transform.position) < 1.3f)
            {
                if (time_to_canPush > 1)
                {
                    if (controller.GetButton(button_pad.Circle) && controller.CanOverhang() && controller.CanOverhangFoot())
                    {
                        stopTrigger = true;
                        controller.ChangeState<State_Pushing>(lastCollision);
                    }
                }
                else time_to_canPush += Time.fixedDeltaTime;
            }
        }
	}

    void OnCollisionStay(Collision collisionInfo)
    {
        if (!paused && !stopTrigger)
        {
            if (collisionInfo.gameObject.GetComponent<CanPush>())
            {
                lastCollision = collisionInfo.gameObject;

                if ( time_to_canPush > 1)
                {
                    if (controller.GetButton(button_pad.Circle) && controller.CanOverhang() && controller.CanOverhangFoot())//&& controller.GetPrevState() != States.State_Pushing
                    {
                        stopTrigger = true;
                        controller.ChangeState<State_Pushing>(collisionInfo.gameObject);
                    }
                }
                else time_to_canPush += Time.fixedDeltaTime;
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
            else if (other.gameObject.GetComponent<ItemController>())
            {
                if (controller.GetButton(button_pad.Circle)) { stopTrigger = true; controller.ChangeState<State_GetItem>(other.gameObject); }
            }
            else if (other.gameObject.GetComponent<ActionController>())
            {
                ActionController o = other.gameObject.GetComponent<ActionController>();
                if (controller.GetButton(button_pad.Circle) && !o.IsActivated()) { stopTrigger = true; controller.ChangeState<State_PlayAction>(other.gameObject); }
            }
            else if (other.gameObject.GetComponent<CanRead>())
            {
                if (controller.GetButton(button_pad.Circle)) { controller.Say(other.gameObject.GetComponent<CanRead>().text, 3); }
            }
        }
    }

    public override void OnMachineStateExit(States new_state)
    {
        controller.Raise();

        controller.shutup("Mueve capullo, no tengo todo el día!!");
    }
}
