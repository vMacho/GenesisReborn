using UnityEngine;
using System.Collections;

public class State_HangRope : State 
{
    Rigidbody rope;
    HingeJoint enlace;
    int last_dir = 0;
    bool jump = false;
    float saveTime = 0.25f, actual_saveTime = 0;

    protected override void Awake() 
	{
        base.Awake();

        controller.GetRigidbody().useGravity = false;
	}

    public override void UpdateState()
    {
        if (rope == null)
        {
            if (actual_saveTime >= saveTime) controller.ChangeState<State_Jump>();
            else actual_saveTime += Time.fixedDeltaTime;
        }

        if (!jump)
        {
            if (rope == null)
            {
                Ray r = new Ray(transform.position + Vector3.up * 0.5f, transform.TransformDirection(transform.right) * -controller.GetDir());
                RaycastHit hitInfo;
                Physics.Raycast(r, out hitInfo, 0.25f);

                if (hitInfo.collider)
                {
                    if (hitInfo.collider.tag == "Rope") SetTarget(hitInfo.collider.gameObject);
                }
            }
            else
            {
                if (controller.GetButton(button_pad.Cross))
                {
                    rope.freezeRotation = false;
                    Destroy(enlace);

                    controller.GetRigidbody().useGravity = true;
                    controller.GetRigidbody().AddForce(Vector3.right * 8000 * (controller.GetPad(direcction_pad.Right) + controller.GetPad(direcction_pad.Left)) + Vector3.up * 3000);

                    jump = true;
                    Invoke("Jump", 1);
                }
                else if (controller.GetPad(direcction_pad.Left) < -0.6f && last_dir != -1)
                {
                    last_dir = -1;
                    controller.GetRigidbody().AddForce(Vector3.left * 5000);
                }
                else if (controller.GetPad(direcction_pad.Right) > 0.6f && last_dir != 1)
                {
                    last_dir = 1;
                    controller.GetRigidbody().AddForce(Vector3.right * 5000);
                }
                else if (controller.GetPad(direcction_pad.Down) < -0.6f)
                {
                    Ray r = new Ray(transform.position + (Vector3.up * 0.25f), transform.TransformDirection(transform.right) * -controller.GetDir());
                    RaycastHit hitInfo;
                    Physics.Raycast(r, out hitInfo, 1.5f);

                    if (hitInfo.collider)
                    {
                        if (hitInfo.collider.tag == "Rope")
                        {
                            enlace.connectedBody = null;
                            controller.transform.position -= Vector3.up * Time.fixedDeltaTime;
                            enlace.connectedBody = controller.GetRigidbody();

                            if (rope != hitInfo.collider.GetComponent<Rigidbody>()) SetTarget(hitInfo.collider.gameObject);
                        }
                    }
                }
                else if (controller.GetPad(direcction_pad.Up) > 0.6f)
                {
                    Ray r = new Ray(transform.position + (Vector3.up), transform.TransformDirection(transform.right) * -controller.GetDir());
                    RaycastHit hitInfo;
                    Physics.Raycast(r, out hitInfo, 1.5f);

                    if (hitInfo.collider)
                    {
                        if (hitInfo.collider.tag == "Rope")
                        {
                            enlace.connectedBody = null;
                            controller.transform.position += Vector3.up * Time.fixedDeltaTime;
                            enlace.connectedBody = controller.GetRigidbody();

                            if (rope != hitInfo.collider.GetComponent<Rigidbody>()) SetTarget(hitInfo.collider.gameObject);
                        }
                    }
                }

            }
        }
        else
        {
            Ray r = new Ray(transform.position + Vector3.up, transform.TransformDirection(transform.right) * -controller.GetDir());
            RaycastHit hitInfo;
            Physics.Raycast(r, out hitInfo, 0.25f);

            if (hitInfo.collider)
            {
                if (hitInfo.collider.tag == "Rope" && rope.transform.parent != hitInfo.collider.transform.parent)
                {
                    jump = false;
                    CancelInvoke("Jump");

                    SetTarget(hitInfo.collider.gameObject);
                }
            }
        }
	}

    public void Jump() { controller.ChangeState<State_Jump>(); }

    void SetTarget(GameObject target)
    {
        if (rope != null)
        {
            rope.freezeRotation = false;
            Destroy(enlace);
        }

        rope = target.GetComponent<Rigidbody>();

        enlace = target.AddComponent<HingeJoint>();
        enlace.connectedBody = controller.GetRigidbody();
        enlace.axis = Vector3.forward;

        rope.freezeRotation = true;

        actual_saveTime = 0;
    }
    
    public override void OnMachineStateExit(States new_state)
    {
        controller.GetRigidbody().useGravity = true;
    }
}
