using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestructibleObject : MonoBehaviour, Attackable
{
    MachineStateController MachineState;

    public float health { get; set; }
    public float maxhealth { get; set; }
    public float maxCombo { get; set; }
    public bool IsAttacking { get; set; }
    public int actualCombo { get; set; }

    void Awake()
    {
        MachineState = gameObject.AddComponent<MachineStateController>();
        health = maxhealth = 5;
        MachineState.PauseAnimator();
    }
	

    void Update() 
    {
        if (health <= 0 && MachineState.GetState() != States.State_Dead)
        {
            MachineState.ResumeAnimator();
            MachineState.ChangeState<State_Dead>();
        }
	}

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Hit(){}

    public void Hurt(float damage)
    {
        Debug.Log("HERIDO DESTRUCTIBLE OBJECT");
        health = Mathf.Max(0, health - damage);
    }
}
