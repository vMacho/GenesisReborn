using UnityEngine;
using System.Collections;
using System;

public class MachineStateController : MonoBehaviour 
{
    States prev_state, actual_state;
    State _state;

    Animator animator;

	void Awake () 
	{
        animator = GetComponent<Animator>();
	}

    public void UpdateState() 
	{
        if (GameController.current != null && _state != null)
        {
            if (!GameController.current.IsPaused() && !_state.IsPaused()) _state.UpdateState();
        }
	}

    public States GetLastState() { return prev_state; }
    public States GetState() { return actual_state; }

    public void SetStatePause(bool val) 
    {
        _state.SetStatePause(val);

        if (val) animator.speed = 0;
        else animator.speed = 1;
    }

    public void SetValAnim(string var, int val) { animator.SetInteger(var, val); }
    public void SetValAnim(string var, float val) { animator.SetFloat(var, val); }
    public void SetValAnim(string var, bool val) { animator.SetBool(var, val); }

    public void SetTrigger(string var){ animator.SetTrigger(var); }
    public void ResetTrigger(string var) { animator.ResetTrigger(var); }

    public bool GetBoolVal(string var) { return animator.GetBool(var); }

    public void ChangeState<T>( GameObject other = null ) where T : new()
	{
        String state_component = typeof(T).Name;

		if( state_component != actual_state.ToString() )
		{
            if (actual_state != States.none) 
			{
                _state.OnMachineStateExit((States)System.Enum.Parse(typeof(States), state_component)); //Ejecutamos la última acción del estado, puede ser en función del nuevo estado
				Destroy( gameObject.GetComponent( actual_state.ToString() ) );
				prev_state = actual_state;
			}

            _state = (State)gameObject.AddComponent(typeof(T));
            _state.OnMachineStateEnter(other);

            actual_state = (States)System.Enum.Parse(typeof(States), state_component);
            if (animator != null) SetValAnim("State", (int)actual_state);

            //Debug.Log("ESTOY EN " + actual_state.ToString());
		}
		else Debug.LogWarning("Ya estaba en el estado " + state_component );
	}

    public void SetRootMotion(bool val) { animator.applyRootMotion = val; }

    public void PauseAnimator() { animator.speed = 0; }
    public void ResumeAnimator() { animator.speed = 1; }
    public void SetAnimatorSpeed( float val) { animator.speed = val; }

    public bool GetAnimation( string val ) { return animator.GetCurrentAnimatorStateInfo(0).IsName(val); }


    public bool HasParameterOfType(string var, AnimatorControllerParameterType type) 
    {
        AnimatorControllerParameter[] parameters = animator.parameters;
        foreach (AnimatorControllerParameter currParam in parameters)
        {
            if (currParam.type == type && currParam.name == name)
            {
                return true;
            }
        }
        return false;
    }
}
