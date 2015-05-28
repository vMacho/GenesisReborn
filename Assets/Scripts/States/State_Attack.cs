using UnityEngine;
using System.Collections;

public class State_Attack : State 
{
    float bored_time = 60, reset_time = 0, default_reset_time = 2;
    bool resetCombo;
    Attackable attackData;

    protected override void Awake() 
	{
        base.Awake();

        attackData = gameObject.GetComponent<Attackable>();

        machine.SetRootMotion(true);
	}

    public override void UpdateState()
    {
        if (!attackData.IsAttacking)
        {
            if (!controller.IsLanded()) controller.ChangeState<State_Jump>(); //si estoy cayendo
            else if (controller.GetButton(button_pad.Cross)) controller.Jump(); //si salto
            else if (controller.GetButton(button_pad.Triangle)) //si abro el inventario
            {
                controller.ChangeState<State_SearchingBag>();
                GameController.current.OpenInventory(controller.GetInvetory(), controller, InventoryMode.self);
            }
            else if (controller.GetButton(button_pad.Square) && !resetCombo)
            {
                if (attackData.actualCombo < attackData.maxCombo)
                {
                    attackData.IsAttacking = true;
                    attackData.actualCombo += 1;
                    machine.SetValAnim("AttackCombo", attackData.actualCombo);
                }
                else resetCombo = true;
            }
            else if (controller.GetPad(direcction_pad.Left) <= -0.6f || controller.GetPad(direcction_pad.Right) >= 0.6f) controller.ChangeState<State_Walk>(); //si ando
            else //Update Attack
            {
                bored_time -= Time.fixedDeltaTime;

                if (bored_time <= 0) machine.SetTrigger("Holster");
            }

            if (resetCombo)
            {
                reset_time += Time.fixedDeltaTime;
                if (reset_time >= default_reset_time)
                {
                    reset_time = default_reset_time;
                    resetCombo = false;
                    attackData.actualCombo = 0;
                }
            }
        }
    }

    public override void OnMachineStateExit(States new_state)
    {
        machine.SetRootMotion(false);
        attackData.actualCombo = 0;
        machine.ResetTrigger("Holster");
    }
}
