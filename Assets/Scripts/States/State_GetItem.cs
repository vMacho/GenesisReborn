using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class State_GetItem : State 
{
    bool finished;
    ItemController _item = null;

    protected override void Awake() 
	{
        base.Awake();

        controller.GetRigidbody().velocity = Vector3.zero;
	}

    public override void UpdateState() 
	{
        if (finished)
        {
            if (_item != null)
            {
                if (controller.AddItem(_item.GetData()))
                {
                    if(this.tag == "Player") //si soy el player
                    {
                        QuestResolve q = _item.gameObject.GetComponent<QuestResolve>();
                        if (q != null) //si tiene una mision para resolver
                        {
                            if (q.type == QuestResolveType.GetItem) q.Resolve();
                        }

                        QuestReciever qr = _item.gameObject.GetComponent<QuestReciever>();
                        if (qr != null) //si tiene una mision para recibir
                        {
                            if (qr.type == QuestRecieverType.GetItem && !GameController.current.player.GetQuest(qr.quest.code)) GameController.current.player.AddQuest(qr.quest);
                        }
                    }

                    Destroy(_item.gameObject);
                    _item = null;
                }
            }

            controller.ChangeState<State_Idle>();
        }
	}

    public void ItemPicked()
    {
        finished = true;
    }

    public override void OnMachineStateEnter(GameObject other)
    {
        base.OnMachineStateEnter(other);

        _item = interactuable.GetComponent<ItemController>();
    }

    public override void OnMachineStateExit(States new_state)
    {
        
    }
}
