//UI_DamageText
/******************** VICTOR MACHO **************/
//Texto con info sobre el daño o curas que recibimos
/***********************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DamageText : MonoBehaviour
{
    public Color dmg_color, health_color;

    void Start()
    {
        Invoke("DestroyMe", 1);
    }

    public void SetDamage(float dmg)
    {
        Text t = GetComponentInChildren<Text>();
        t.text = "-" + dmg.ToString();
        t.color = dmg_color;
    }

    public void SetHealth(float dmg)
    {
        Text t = GetComponentInChildren<Text>();
        t.text = "+" + dmg.ToString();
        t.color = health_color;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}