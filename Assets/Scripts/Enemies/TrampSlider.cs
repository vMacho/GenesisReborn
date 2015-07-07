using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrampSlider : MonoBehaviour
{
    public int Damage = 100;
    public float Timer = 1;
    float actual_timer = 0;

    void FixedUpdate()
    {
        if (actual_timer < Timer) actual_timer += Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (actual_timer >= Timer)
        {
            Attackable o = other.GetComponent<Attackable>();
            if (o != null) o.Hurt(Damage);

            actual_timer = 0;
        }
    }
}