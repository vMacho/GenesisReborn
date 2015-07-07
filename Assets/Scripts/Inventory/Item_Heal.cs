using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item_Heal : Item 
{
    float healing;

    public Item_Heal(float h) { healing = h; }

    public override bool Use(Controller target)
    {
        if (target.GetComponent<Attackable>() != null)
        {
            Attackable t = target.GetComponent<Attackable>();

            if (t.health < t.maxhealth)
            {
                t.Health(healing);
                
                return true;
            }
            else return false;
        }
        else return false;
    }
}
