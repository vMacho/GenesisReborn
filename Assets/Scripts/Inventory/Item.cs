using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public abstract class Item
{
    public string description, obj, img;
    public int value;

    public Item()
    {
        description = "vacio";
        img = null;
        value = 0;
        obj = "";
    }

    public virtual bool Use(Controller target) { throw new NotImplementedException(); }
}
