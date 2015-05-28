using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Item_Armor : Item
{
    int def;
    Armor_Slot slot;
    Special_Armor_Effect special;

    public Item_Armor(int d, Armor_Slot s, Special_Armor_Effect sp) 
    {
        def = d;
        slot = s;
        special = sp;
    }

    public Armor_Slot GetSlot() { return slot; }
    public bool IsSpecial( Special_Armor_Effect e) { return (special == e); }

    public override bool Use(Controller target)
    {
        target.SetArmor(this);
        return true;
    }

    public int GetDef() { return def; }
}

[System.Serializable]
public enum Armor_Slot
{
    Head,
    Chest,
    Arms,
    Legs,
    Boots
}

[System.Serializable]
public enum Special_Armor_Effect
{
    none,
    propulsor
}