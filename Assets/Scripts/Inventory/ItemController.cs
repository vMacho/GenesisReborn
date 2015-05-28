using UnityEngine;
using System.Collections;
using System;

public class ItemController : MonoBehaviour 
{
    GameObject aviso;

    public ItemType itemtipo;
    Item _item;

    public string description, obj;
    public Sprite img;
    public int value;

    [Header("Item Bag")]
    public int size = 0;

    [Header("Item Weapon")]
    public int damage = 0;
    public float distance = 0;
    public GameObject RenderMesh;

    [Header("Item Armor")]
    public int def;
    public Armor_Slot slot;
    public Special_Armor_Effect special;

    [Header("Item Heal")]
    public float healing;

    protected virtual void Start() 
    {
        switch (itemtipo)
        {
            case ItemType.Armor:
                _item = new Item_Armor(def, slot, special);
                break;
            case ItemType.Bag:
                _item = new Item_Bag( size );
                break;
            case ItemType.Heal:
                _item = new Item_Heal( healing );
                break;
            case ItemType.Weapon:
                _item = new Item_Weapon(damage, distance, RenderMesh.name);
                break;
        }

        _item.description = description;
        _item.obj = obj;
        _item.img = img.name;
        _item.value = value;

        aviso = transform.Find("Canvas").gameObject;

        gameObject.layer = 10;
	}

    public Item GetData() { return _item; }

    void OnTriggerStay(Collider other)
    {
        Player c = other.GetComponent<Player>();
        if (!aviso.activeSelf && c)
        {
            if (c.GetState() == States.State_Idle || c.GetState() == States.State_Walk) aviso.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Player c = other.GetComponent<Player>();
        if (c) aviso.SetActive(false);
    }
}
