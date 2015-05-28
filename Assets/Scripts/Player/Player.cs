using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Controller, Attackable 
{
    List<Quest> quest = new List<Quest>();

    public float health { get; set; }
    public float maxhealth { get; set; }
    public float maxCombo { get; set; }
    public bool IsAttacking { get; set; }
    public int actualCombo { get; set; }

    protected override void Awake() 
	{
        base.Awake();

        inventory = new Inventory(new Item_Bag(0));
        weapon = null;

        armor = new List<Item_Armor>(5);
        armor.Add(new Item_Armor(0, Armor_Slot.Head, Special_Armor_Effect.none));
        armor.Add(new Item_Armor(0, Armor_Slot.Chest, Special_Armor_Effect.none));
        armor.Add(new Item_Armor(0, Armor_Slot.Arms, Special_Armor_Effect.none));
        armor.Add(new Item_Armor(0, Armor_Slot.Legs, Special_Armor_Effect.none));
        armor.Add(new Item_Armor(0, Armor_Slot.Boots, Special_Armor_Effect.none));

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
        maxCombo = 3;

        ChangeState<State_Idle>();
	}
    
    void Update()
    {
        UpdateControls();

        if (Mando.GetButton(button_pad.Start)) GameController.current.SetGamePause(!GameController.current.IsPaused());

        if (GetWeapon() != null)
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(transform.right) * -GetDir() * GetWeapon().GetRange(), Color.green);
        }
    }

    void UpdateControls()
	{        
        /**** PAD ***/
        Mando.SetPad(direcction_pad.Right, (Input.GetAxis("Horizontal") > 0) ? Input.GetAxis("Horizontal") : 0);
        Mando.SetPad(direcction_pad.Left, (Input.GetAxis("Horizontal") < 0) ? Input.GetAxis("Horizontal") : 0);

        Mando.SetPad(direcction_pad.Up, Input.GetAxis("Vertical"));
        Mando.SetPad(direcction_pad.Down, Input.GetAxis("Vertical"));

        Mando.SetPad(direcction_pad.Right_2, Input.GetAxis("Horizontal_Right"));
        Mando.SetPad(direcction_pad.Left_2, Input.GetAxis("Horizontal_Right"));

        Mando.SetPad(direcction_pad.Up_2, Input.GetAxis("Vertical_Right"));
        Mando.SetPad(direcction_pad.Down_2, Input.GetAxis("Vertical_Right"));
        /**************************************************************************/

        /*** Buttons **/
        Mando.SetButton(button_pad.Cross, (Input.GetButton("Jump") ));

        Mando.SetButton(button_pad.Square, (Input.GetButtonDown("Attack") ));

        Mando.SetButton(button_pad.Circle, (Input.GetButtonDown("Action") ));

        Mando.SetButton(button_pad.Triangle, Input.GetButtonDown("Inventory"));

        Mando.SetButton(button_pad.Start, (Input.GetButtonDown("Start") ));

        Mando.SetButton(button_pad.R2, (Input.GetAxis("Dash") <= -0.5f));

        Mando.SetButton(button_pad.R3, Input.GetButtonDown("Quest"));
		/**************************************************************************/
	}

    public void AddQuest(Quest q) //añade una quest
    {
        if (!GetQuest(q.code))
        {
            Vector3 pos = transform.position + Vector3.up;
            Instantiate(Resources.Load("UI/NewQuest"), pos, Quaternion.identity);
            Debug.Log("Nueva Quest!!");

            quest.Add(q);
        }
    }

    public bool GetQuest(int c) //si tiene un quest en concreto
    {
        foreach (Quest q in quest)
        {
            if (q.code == c) return true;
        }
        return false;
    }

    public List<Quest> GetQuest(){ return quest; } //listado de quest

    public bool GetQuestDone(int c) //si tiene hecha dicha quest
    {
        foreach (Quest q in quest) 
        {
            if (q.code == c) return q.IsDone();
        }

        return false; 
    }
    
    public void FinishQuest(Quest qr) //finaliza una quest aunque no la tuviese
    {
        bool found = false;
        foreach (Quest q in quest)
        {
            if (q.code == qr.code)
            {
                Vector3 pos = gameObject.transform.position + Vector3.up;
                Instantiate(Resources.Load("UI/FinishQuest"), pos, Quaternion.identity);
                Debug.Log("Quest hecha!");

                q.SetDone();
                found = true;
                break;
            }
        }

        if (!found) //Si no encuentra la quest (la agregamos y la completamos)
        {
            quest.Add(qr);
            FinishQuest(qr);
        }
    }

    public void Kill()
    {
        GameController.current.LoadCheckPoint();
    }

    public void Hit()
    {
        IsAttacking = false;
        MachineState.SetValAnim("AttackCombo", 0);

        Ray r = new Ray(transform.position + Vector3.up, transform.TransformDirection(transform.right) * -GetDir());
        RaycastHit hitInfo;
        Physics.Raycast(r, out hitInfo, GetWeapon().GetRange(), landed_layerMask);

        if (hitInfo.collider)
        {
            Attackable other = hitInfo.collider.GetComponent<Attackable>();

            if (other != null)
            {
                other.Hurt(GetWeapon().GetDamage());
            }
        }
    }

    public void Hurt( float d )
    {
        float damage = d - GetArmorDefence();
        if (damage > 0)
        {
            health = Mathf.Max(0, health - damage);

            GameController.current.UpdateHealth();

            Debug.Log("HERIDO PLAYER " + damage);
        }
        else Debug.Log("No atraviesa la defensa");
    }
}