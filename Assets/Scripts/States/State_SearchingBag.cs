using UnityEngine;
using System.Collections;

public class State_SearchingBag : State 
{
    float bored_time = 20, bored_time_actual = 0;
    bool bored;
    int mode = 0;

    protected override void Awake() 
	{
        base.Awake();

        machine.ResetTrigger("Bored");
	}

    public void SetMode(int m) { mode = m; }

    public override void UpdateState() 
	{
        bored_time_actual += Time.fixedDeltaTime;

        if (!controller.IsLanded()) controller.ChangeState<State_Jump>(); //si estoy cayendo
        if (mode == 1 && controller.GetButton(button_pad.Triangle)) controller.ChangeState<State_Idle>(); //si salgo del inventario
        if (mode == 2 && controller.GetButton(button_pad.R3)) controller.ChangeState<State_Idle>(); //si salgo del menú Quest
        else if (!bored && bored_time_actual >= bored_time && mode == 1) //si no hago nada y estoy en el inventario
        {
            bored = true;
            machine.SetTrigger("Bored");
            controller.Say("¿Donde está la maldita cosa que busco?", 4);
        }
        else
        {
            float mod = 0.8f;
            controller.Move(mod);

            float speed = Mathf.Abs(controller.GetRigidbody().velocity.x);

            if (speed == 0) speed = -0.1f;
            machine.SetValAnim("Speed_X", speed);
        }
	}

    public override void OnMachineStateExit(States new_state)
    {
        if (mode == 1) GameController.current.CloseInventory();
        else if (mode == 2) GameController.current.CloseQuests();

        controller.shutup("¿Donde está la maldita cosa que busco?");
    }
}
