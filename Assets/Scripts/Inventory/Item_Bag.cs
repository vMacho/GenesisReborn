using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Item_Bag : Item
{
    int size = 0;

    public Item_Bag(int s) { size = s; }
    public Item_Bag(Item_Bag c) 
    { 
        size = c.GetSize();
        description = c.description;
        value = c.value;
        img = c.img;
        obj = c.obj;
    }

    public int GetSize() { return size; }

    public override bool Use(Controller target)
    {
        target.GetInvetory().UpgradeBag(this);
        return true;
    }
}