using UnityEngine;
using System.Collections;

public abstract class State : MonoBehaviour 
{
	protected Controller controller;
    protected bool paused;
    protected bool stopTrigger;
    protected MachineStateController machine;
    protected GameObject interactuable;

	protected virtual void Awake () 
	{
        controller = GetComponent<Controller>();
        machine = GetComponent<MachineStateController>();
	}

    public void SetStatePause(bool val) { paused = val; }
    public bool IsPaused() { return paused; }

    public virtual void OnMachineStateEnter(GameObject other){ interactuable = other; }

    public abstract void OnMachineStateExit(States new_state);
    public abstract void UpdateState();
}

public enum States
{
    none,
    State_Idle,
    State_Jump,
    State_Walk,
    State_UpStairs,
    State_Overhang,
    State_GetItem,
    State_SearchingBag,
    State_WallSlide,
    State_Pushing,
    State_Attack,
    State_Dead,
    State_HangRope,
    State_PlayAction,
    State_Speak
}