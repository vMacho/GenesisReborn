using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Inventory 
{
    Item_Bag bag;
    List<Item> items;

    public Inventory(Item_Bag b, List<Item> i)
    {
        bag = b;
        items = new List<Item>(i);
    }

    public Inventory(Item_Bag b)
    {
        bag = b;
        items = new List<Item>();
    }

    public bool IsFull() { return (items.Count >= bag.GetSize()); }
    public void UpgradeBag(Item_Bag c) { bag = new Item_Bag(c);}
    public Item_Bag GetBag() { return bag; }
    public int Size() { return bag.GetSize(); }
    public bool AddItem(Item i) 
    {
        if (items.Count + 1 <= bag.GetSize())
        {
            items.Add(i);
            return true;
        }
        else return false;
    }
    public void RemoveItem(Item i) { items.Remove(i); }
    public List<Item> OpenBag() { return items; }

    public string GetImage() { return bag.img; }
}