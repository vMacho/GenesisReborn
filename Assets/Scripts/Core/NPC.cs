using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : Controller 
{
    bool muted;
    public List<DialogTree> dialog = new List<DialogTree>();

    GameObject action_canvas;

    protected override void Awake() 
	{
        base.Awake();

        action_canvas = transform.FindChild("Canvas").gameObject;

        inventory = new Inventory(new Item_Bag(0));

		_aceleration = 9f;
        _run_aceleration = 18f;
		_jump = 450.0f;
        _jump_force = 20f;
        _speed = 6f;
        _upstairs_speed = 100f;
        _downstairs_speed = 60f;

        ChangeState<State_Idle>();
	}

    public void SetMuted(){ muted = true;}

    protected override void FixedUpdate()
	{
        UpdateIA();
        base.FixedUpdate();

        
	}

    void UpdateIA()
	{
        if (!muted && Vector3.Distance(transform.position, GameController.current.player.transform.position) < 2.1f && GameController.current.player.GetState() != States.State_Speak)
        {
            action_canvas.SetActive(true);

            if (GameController.current.player.GetButton(button_pad.Circle))
            {
                GameController.current.player.ChangeState<State_Speak>( this.gameObject );

                ChangeState<State_Speak>(GameController.current.player.gameObject);
            }
        }
        else action_canvas.SetActive(false);
	}

    public int Speak()
    {
        for (int i = 0; i < dialog.Count; ++i )
        {
            if (dialog[i].quest.code == 0 || !GameController.current.player.GetQuestDone(dialog[i].quest.code)) //si el dialogo no tiene quest o no la tiene hecha
            {
                for (int j = 0; j < dialog[i].dialog.Count; ++j)
                {
                    if (!dialog[i].dialog[j].Said)
                    {
                        Say(dialog[i].dialog[j].Text, dialog[i].dialog[j].Time);

                        if (!dialog[i].Repeteable) dialog[i].dialog[j] = new Dialog(dialog[i].dialog[j].Text, dialog[i].dialog[j].Time, true);

                        //dialog[i].dialog[j] = new Dialog(dialog[i].dialog[j].Text, dialog[i].dialog[j].Time, true, dialog[i].dialog[j].Repeteable);

                        return i;
                    }
                }
            }
            else //ponemos el dialogo como dicho
            {
                for (int j = 0; j < dialog[i].dialog.Count; ++j)
                {
                    if (!dialog[i].dialog[j].Said)
                    {
                        if (!dialog[i].Repeteable) dialog[i].dialog[j] = new Dialog(dialog[i].dialog[j].Text, dialog[i].dialog[j].Time, true);

                        //dialog[i].dialog[j] = new Dialog(dialog[i].dialog[j].Text, dialog[i].dialog[j].Time, true, dialog[i].dialog[j].Repeteable);
                    }
                }
            }
        }

        return -1;
    }
}