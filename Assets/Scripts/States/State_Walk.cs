using UnityEngine;
using System.Collections;

public class State_Walk : State 
{
    protected override void Awake() 
	{
        base.Awake();
	}

    public override void UpdateState() 
	{
        Rigidbody body = controller.GetRigidbody();

        if (!controller.IsLanded()) controller.ChangeState<State_Jump>(); //Si estoy cayendo
        else if (controller.GetButton(button_pad.Cross) && !controller.IsBlendDown() ) controller.Jump(); //si salto
        else if (controller.GetButton(button_pad.Triangle)) //Si abro el inventario
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
        else if ((controller.GetPad(direcction_pad.Down) <= -0.45f && controller.GetButton(button_pad.R2)) || controller.IsBlendDown())
        {
            if (!controller.IsBlendDown()) controller.BlendDown();
            
            controller.Rotate(); //Si esta rodando
        }
        else if (Mathf.Abs(controller.GetPad(direcction_pad.Left) + controller.GetPad(direcction_pad.Right)) <= 0.7f && Mathf.Abs(body.velocity.x) < 0.35f) //si no me muevo
        {
            controller.ChangeState<State_Idle>();
        }
        else //MOVE
        {
            if (controller.GetButton(button_pad.R2)) controller.Run();
            else controller.Move();

            machine.SetValAnim("Speed_X", Mathf.Abs(body.velocity.x));
        }
	}

    void OnTriggerStay(Collider other)
    {
        if (!paused && !controller.GetDash() && !stopTrigger)
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
                if (controller.GetButton(button_pad.Circle)) { stopTrigger = true; controller.Say(other.gameObject.GetComponent<CanRead>().text, 3); }
            }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (!paused && !stopTrigger)
        {
            if (collisionInfo.gameObject.GetComponent<CanPush>())
            {
                if (controller.GetButton(button_pad.Circle) && controller.GetPrevState() != States.State_Pushing && controller.CanOverhang() && controller.CanOverhangFoot())
                {
                    stopTrigger = true;
                    controller.ChangeState<State_Pushing>(collisionInfo.gameObject);
                }
            }
        }
    }

    public override void OnMachineStateExit(States new_state)
    {
        
    }
}
