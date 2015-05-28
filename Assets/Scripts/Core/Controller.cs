using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    int _dir = 1;
    protected float _aceleration, _run_aceleration, _force, _jump, _jump_force, _speed, _upstairs_speed, _downstairs_speed;
    protected bool can_jump, can_run, can_fly, can_bend, _landed, _canOverHang, _canOverhangFoot, _inDash, _armed, _blendDown;

    protected Vector3 collision_center;
    protected float collision_height;

    protected ControllerPad Mando;

    protected MachineStateController MachineState;

    protected Rigidbody body;
    protected Transform hand_slot, draw_slot, actual_weapon;

    protected Inventory inventory;
    protected List<Item_Armor> armor;
    protected Item_Weapon weapon;

    protected LayerMask landed_layerMask;

    Vector3 prev_velocity = Vector3.zero;

    protected virtual void Awake()
    {
        Mando = new ControllerPad();

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.name == "weapon_slot") hand_slot = t;
            else if (t.name == "draw_slot") draw_slot = t;
        }

        collision_center = GetComponent<CapsuleCollider>().center;
        collision_height = GetComponent<CapsuleCollider>().height;

        body = gameObject.AddComponent<Rigidbody>();
        body.mass = 10;
        body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        body.drag = 1;
        body.angularDrag = 0;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        MachineState = gameObject.AddComponent<MachineStateController>();

        gameObject.layer = 8;
        Physics.IgnoreLayerCollision(8, 8);

        landed_layerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("WallSlide"));
    }

    public LayerMask GetLayerMask() { return landed_layerMask; }

    public Rigidbody GetRigidbody() { return body; }

    public float GetPad(direcction_pad direcction) { return Mando.GetPad(direcction); }
    public bool GetButton(button_pad button) { return Mando.GetButton(button); }

    public float GetAceleration() { return _aceleration; }
    public float GetSpeedDash() { return _speed * 50f; }
    public int GetDir() { return _dir; }
    public void SetDir(int val) { _dir = val; transform.localEulerAngles = new Vector3(0, 90, 0) * _dir; }
    public bool GetDash() { return _inDash; }

    public bool IsBlendDown() { return _blendDown; }

    public States GetPrevState() { return MachineState.GetLastState(); }
    public States GetState() { return MachineState.GetState(); }
    public void ChangeState<T>(GameObject other = null) where T : new() { MachineState.ChangeState<T>(other); }

    public bool IsLanded() { return _landed; }
    public bool CanOverhang() { return _canOverHang; }
    public bool CanOverhangFoot() { return _canOverhangFoot; }

    public Item_Armor GetArmor(Armor_Slot s) { return armor[(int)s]; }
    public bool HasSpecialArmor(Special_Armor_Effect e)
    {
        for (int i = 0; i <= armor.Count - 1; ++i)
        {
            if (armor[i].IsSpecial(e)) return true;
        }

        return false;
    }

    public int GetArmorDefence() 
    {
        int df = 0;

        for (int i = 0; i <= armor.Count - 1; ++i)
        {
            df += armor[i].GetDef();
        }

        return df;
    }

    public void SetArmor(Item_Armor a)
    {
        if (armor[(int)a.GetSlot()].obj != "") AddItem(armor[(int)a.GetSlot()]);
        armor[(int)a.GetSlot()] = a;
    }

    public Item_Weapon GetWeapon() { return weapon; }

    public void AddWeapon(Item_Weapon w)
    {
        foreach (Transform child in hand_slot.transform) Destroy(child.gameObject);

        GameObject we = Instantiate(Resources.Load("Mesh/" + w.GetRenderMesh()), hand_slot.transform.position, hand_slot.transform.rotation) as GameObject;
        
        we.transform.parent = draw_slot;
        we.transform.localScale = Vector3.one;
        we.transform.localRotation = Quaternion.identity;
        we.transform.localPosition = Vector3.zero;

        weapon = w;

        actual_weapon = we.transform;
    }

    public bool HasWeapon() { return (weapon != null); }

    public void DrawYourWeapon()
    {
        if (actual_weapon != null)
        {
            actual_weapon.parent = hand_slot;
            actual_weapon.localScale = Vector3.one;
            actual_weapon.localRotation = Quaternion.identity;
            actual_weapon.localPosition = Vector3.zero;

            _armed = true;
            MachineState.SetValAnim("Armed", true);
        }
    }

    public void HolsterWeapon()
    {
        actual_weapon.parent = draw_slot;
        actual_weapon.localScale = Vector3.one;
        actual_weapon.localRotation = Quaternion.identity;
        actual_weapon.localPosition = Vector3.zero;

        _armed = false;
        MachineState.SetValAnim("Armed", false);

        ChangeState<State_Idle>();
    }

    protected virtual void FixedUpdate()
    {
        if (!can_fly) UpdateLanded();
        else _landed = true;

        MachineState.UpdateState();
    }

    void UpdateLanded()
    {
        bool toca_tierra = false;
        for (int i = 0; i < 3; ++i)
        {
            float x = (transform.position.x - transform.lossyScale.x / 4) + transform.lossyScale.x / 4 * i;

            //Debug.DrawRay(new Vector3(x, transform.position.y - transform.localScale.y * 0.75f, 0), transform.TransformDirection(transform.up * -1) * 0.3f, Color.red);
            if (Physics.Raycast(new Vector3(x, transform.position.y - transform.localScale.y * 0.75f, 0), transform.TransformDirection(transform.up * -1), 0.3f, landed_layerMask)) toca_tierra = true;
        }
        _landed = toca_tierra;

        //Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(transform.right) * -_dir, Color.green);
        _canOverHang = (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(transform.right) * -_dir, 1, landed_layerMask)) ? true : false;

        //Debug.DrawRay(transform.position - Vector3.up / 2, transform.TransformDirection(transform.right) * -_dir, Color.green);
        _canOverhangFoot = (Physics.Raycast(transform.position - Vector3.up / 2, transform.TransformDirection(transform.right) * -_dir, 1, landed_layerMask)) ? true : false;

    }

    public void Move(float mod = 1f)
    {
        float velocity_x = _aceleration * (Mando.GetPad(direcction_pad.Right) + Mando.GetPad(direcction_pad.Left)) * Time.fixedDeltaTime;

        Vector3 newspeed = body.velocity + new Vector3(velocity_x * mod, 0, 0);

        if (Mathf.Abs(newspeed.x) > _speed) newspeed.x = _speed * Mathf.Sign(newspeed.x);

        body.velocity = newspeed;
        
        if (GetPad(direcction_pad.Left) < 0)
        {
            SetDir(-1);
            MachineState.SetValAnim("Dir", -1);
        }
        else if (GetPad(direcction_pad.Right) > 0)
        {
            SetDir(1);
            MachineState.SetValAnim("Dir", 1);
        }
    }

    public void Run(float mod = 1f)
    {
        float velocity_x = _run_aceleration * (Mando.GetPad(direcction_pad.Right) + Mando.GetPad(direcction_pad.Left)) * Time.fixedDeltaTime;

        Vector3 newspeed = body.velocity + new Vector3(velocity_x * mod, 0, 0);

        if (Mathf.Abs(newspeed.x) > _speed) newspeed.x = _speed * Mathf.Sign(newspeed.x);

        body.velocity = newspeed;
        
        if (GetPad(direcction_pad.Left) < 0)
        {
            SetDir(-1);
            MachineState.SetValAnim("Dir", -1);
        }
        else if (GetPad(direcction_pad.Right) > 0)
        {
            SetDir(1);
            MachineState.SetValAnim("Dir", 1);
        }
    }

    public void Push(int dir)
    {
        float velocity_x = _aceleration * (Mando.GetPad(direcction_pad.Right) + Mando.GetPad(direcction_pad.Left)) * Time.fixedDeltaTime;

        Vector3 newspeed = body.velocity + new Vector3(velocity_x, 0, 0);

        if (Mathf.Abs(newspeed.x) > _speed) newspeed.x = _speed * Mathf.Sign(newspeed.x);

        body.velocity = newspeed;

        if (GetPad(direcction_pad.Right) > 0)
        {
            MachineState.ResumeAnimator();

            if (dir == 1) MachineState.SetValAnim("Pushing", true);
            else MachineState.SetValAnim("Pushing", false);

            MachineState.SetAnimatorSpeed(Mathf.Abs(GetPad(direcction_pad.Right)));
        }
        else if (GetPad(direcction_pad.Left) < 0)
        {
            MachineState.ResumeAnimator();

            if (dir == -1) MachineState.SetValAnim("Pushing", true);
            else MachineState.SetValAnim("Pushing", false);

            MachineState.SetAnimatorSpeed(Mathf.Abs(GetPad(direcction_pad.Left)));
        }
        else MachineState.PauseAnimator();        
    }

    public void StartDash()
    {
        MachineState.SetTrigger("Dash");
        _inDash = true;
    }

    public void Dash()
    {
        body.AddForce(new Vector3(body.velocity.normalized.x * GetSpeedDash(), 0, 0));
    }

    public void EndDash()
    {
        _inDash = false;
        MachineState.ResetTrigger("Dash");
    }

    public void Rotate()
    {
        float velocity_x = _aceleration * 1.5f * _dir * Time.fixedDeltaTime;
        Vector3 newspeed = body.velocity + new Vector3(velocity_x, 0, 0);
        _blendDown = true;
        body.velocity = newspeed;

        MachineState.SetValAnim("BlendDown", _blendDown);
    }

    public void BlendedDown(){ _blendDown = true; }

    public void BlendDown()
    {
        gameObject.GetComponent<CapsuleCollider>().center = new Vector3(collision_center.x, -collision_height * 0.25f, collision_center.z);
        gameObject.GetComponent<CapsuleCollider>().height = collision_height/2;
        //_blendDown = false;
        MachineState.SetValAnim("BlendDown", true);
    }

    public void Raise()
    {
        GetComponent<CapsuleCollider>().center = collision_center;
        GetComponent<CapsuleCollider>().height = collision_height;

        _blendDown = false;

        MachineState.SetValAnim("BlendDown", _blendDown);
    }

    public void Jump( )
    {
        float velocity_x = _aceleration * (Mando.GetPad(direcction_pad.Right) + Mando.GetPad(direcction_pad.Left)) * Time.fixedDeltaTime;
        float newspeed_x = body.velocity.x + velocity_x * _jump_force;
        float salto = (_blendDown)? _jump * 1.1f : _jump;

        body.velocity = new Vector3(newspeed_x, salto * Time.fixedDeltaTime, body.velocity.z);

        ChangeState<State_Jump>();
    }

    public void DoubleJump()
    {
        float velocity_x = _aceleration * (Mando.GetPad(direcction_pad.Right) + Mando.GetPad(direcction_pad.Left)) * Time.fixedDeltaTime;
        float newspeed_x = body.velocity.x + velocity_x * _jump_force;

        body.velocity = new Vector3(newspeed_x, _jump * 0.75f * Time.fixedDeltaTime, body.velocity.z);
    }

    public void UpStairs(float mod, GameObject stair = null)
    {
        float velocity_y = 0, velocity_x = 0;

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.75f, 0), transform.TransformDirection(-transform.forward) *2.1f, Color.red);
        bool canUp = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.75f, 0), transform.TransformDirection(-transform.forward), 2.1f);
        
        if (Mando.GetPad(direcction_pad.Up) >= 0.6f & canUp) velocity_y = _upstairs_speed;
        else if (Mando.GetPad(direcction_pad.Down) <= -0.6f && !_landed) velocity_y = -_downstairs_speed;

        if (Mathf.Abs(Mando.GetPad(direcction_pad.Right) + Mando.GetPad(direcction_pad.Left)) > 0.2f) //si me muevo hacia los lados
        {
            velocity_x = _aceleration * (Mando.GetPad(direcction_pad.Right) + Mando.GetPad(direcction_pad.Left)) * Time.fixedDeltaTime;
        }
        else //lo llevamos hacia el centro
        {

        }

        float newspeed_x = velocity_x * mod;

        if (Mathf.Abs(newspeed_x) > _speed) newspeed_x = _speed * Mathf.Sign(newspeed_x);

        body.velocity = new Vector3(newspeed_x, velocity_y * Time.fixedDeltaTime, 0);

        if (MachineState.GetAnimation("upstairs_move") || MachineState.GetAnimation("downstairs_move") || MachineState.GetAnimation("upstairs_move_left") || MachineState.GetAnimation("downstairs_move_left"))
        {
            MachineState.SetRootMotion(false);
            if (Mando.GetPad(direcction_pad.Up) > 0)
            {
                MachineState.ResumeAnimator();
                MachineState.SetValAnim("Dir_vertical", 1);
            }
            else if (!IsLanded() && Mando.GetPad(direcction_pad.Down) < 0)
            {
                MachineState.ResumeAnimator();
                MachineState.SetValAnim("Dir_vertical", -1);
            }
            else MachineState.PauseAnimator();
        }
        else MachineState.ResumeAnimator();
    }

    public void Wallflip()
    {
        body.velocity = new Vector3(_aceleration * (GetDir() * -1) * Time.fixedDeltaTime * 20f, _jump * Time.fixedDeltaTime, 0);

        if (GetPad(direcction_pad.Left) < 0)
        {
            SetDir(-1);
            MachineState.SetValAnim("Dir", -1);
        }
        else if (GetPad(direcction_pad.Right) > 0)
        {
            SetDir(1);
            MachineState.SetValAnim("Dir", 1);
        }
    }
    
    void OnPauseGame()
    {
        prev_velocity = body.velocity;
        body.isKinematic = true;
        body.Sleep();
        MachineState.SetStatePause(true);
    }

    void OnResumeGame()
    {
        body.velocity = prev_velocity;
        body.isKinematic = false;
        body.WakeUp();
        MachineState.SetStatePause(false);
    }
    
    public void Say(string text, float time)
    {
        Debug.Log(text);

        bool repetido = false;
        Transform _last = null;
        foreach (ConversationController c in gameObject.GetComponentsInChildren<ConversationController>())
        {
            if (c.GetText() == text)
            {
                c.SetTime(time);
                repetido = true;
                break;
            }
            else _last = c.transform;
        }

        if (!repetido)
        {
            Vector3 pos = transform.position + new Vector3(0.2f, 1f, 0);
            if (_last != null) pos = _last.transform.position + new Vector3(2.2f, 1, 0);

            GameObject canvas_conversation = Instantiate(Resources.Load("UI/BoxSay"), pos, transform.rotation) as GameObject;

            if (_last == null) canvas_conversation.transform.parent = transform;
            else canvas_conversation.transform.parent = _last;

            canvas_conversation.GetComponent<ConversationController>().Initialize(text, time);
        }
    }

    public void shutup(string text = "")
    {
        foreach (ConversationController c in GetComponentsInChildren<ConversationController>())
        {
            if (text == "" || c.GetText() == text) c.ShutUp();
        }
    }

    public bool AddItem(Item i)
    {
        if (inventory.Size() == 0)
        {
            if (i.GetType().IsAssignableFrom(typeof(Item_Bag)))
            {
                UseItem(i);
                return true;
            }
            else
            {
                Say("No tengo una mochila", 3);
                return false;
            }
        }
        else
        {
            if (!inventory.AddItem(i))
            {
                Say("No puedo cargar más objetos", 3);
                return false;
            }
            else return true;
        }
    }

    public bool UseItem(Item i)
    {
        bool result = i.Use(this);

        if (result) inventory.RemoveItem(i);
        else Debug.Log("NO SE PUEDE USAR EL OBJETO");

        return result;
    }

    public bool DropItem(Item i)
    {
        Instantiate(Resources.Load("Items/" + i.obj), transform.position - (Vector3.up * 0.85f), Quaternion.identity);
        inventory.RemoveItem(i);

        return true;
    }

    public Item GetItem(int i)
    {
        return inventory.OpenBag()[i];
    }

    public Inventory GetInvetory() { return inventory; }
    public void SetInventory(Inventory i) { inventory = i; }

    public void RemoveItem(Item i) { inventory.RemoveItem(i); }

    public void ExamineItem(Item i)
    {
        Say(i.description, 3);
    }
}