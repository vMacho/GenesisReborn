using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Item_Weapon : Item
{
    int damage;
    float distance;
    string renderMesh;

    public Item_Weapon(int d, float dis, string r) { damage = d; renderMesh = r; distance = dis; }

    public string GetRenderMesh() { return renderMesh; }
    public int GetDamage() { return damage; }
    public float GetRange() { return distance; }

    public override bool Use(Controller target)
    {
        target.AddWeapon(this);
        return true;
    }
}
public enum WeaponType
{
    melee,
    ranged
}