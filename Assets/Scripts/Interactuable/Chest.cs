using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chest : ActionController
{
    protected override void Awake()
    {
        base.Awake();

        action = ActionType.open_chest;
    }
    
    public override void PlayAction() 
    {
        base.PlayAction();

        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if (child.GetComponent<ItemController>())
            {
                child.gameObject.SetActive(true);

                child.gameObject.GetComponent<Animator>().SetInteger("State", 1);
                child.gameObject.GetComponent<Animator>().SetInteger("random", Random.Range(-1, 2));
            }
        }
    }
}
