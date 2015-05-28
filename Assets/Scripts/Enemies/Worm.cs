using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Worm : Controller, Attackable 
{
    public float health { get; set; }
    public float maxhealth { get; set; }
    public float maxCombo { get; set; }
    public bool IsAttacking { get; set; }
    public int actualCombo { get; set; }

    protected override void Awake() 
	{
        base.Awake();

        armor = new List<Item_Armor>(5);
        armor.Add(new Item_Armor(10, Armor_Slot.Head, Special_Armor_Effect.none));

		_aceleration = 9f;
        _run_aceleration = 18f;
		_jump = 450.0f;
        _jump_force = 20f;
        _speed = 6f;
        _upstairs_speed = 100f;
        _downstairs_speed = 60f;

        health = 100;
        maxhealth = 100;
        actualCombo = 0;
        maxCombo = 1;

        MachineState.ChangeState<State_Idle>();

        GetRigidbody().useGravity = false;
        can_fly = true;

        gameObject.layer = 11;
        Physics.IgnoreLayerCollision(11, 11);
        Physics.IgnoreLayerCollision(11, 0);
	}

    protected override void FixedUpdate()
	{
        UpdateAI();
        base.FixedUpdate();
	}

    void UpdateAI()
	{        
       
	}

    public void Kill()
    {
        
    }

    public void Hit()
    {
        IsAttacking = false;
        
    }

    public void Hurt( float d )
    {
        float damage = d - GetArmorDefence();
        if (damage > 0)
        {
            health = Mathf.Max(0, health - damage);

            Debug.Log("HERIDO "+ gameObject.name + " " + damage);
        }
        else Debug.Log("No atraviesa la defensa");
    }
}