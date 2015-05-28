using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State_Speak : State 
{
    Controller receptor;
    bool speaking = false;

    protected override void Awake() 
	{
        base.Awake();

        controller.GetRigidbody().velocity = Vector3.zero;
	}

    public override void UpdateState()
    {
        if (!speaking)
        {
            if (receptor.GetComponent<NPC>()) //si es un NPC
            {
                int index = receptor.GetComponent<NPC>().Speak();
                if (index != -1) //si tiene algo que decir
                {
                    speaking = true;

                    //comprobamos si tiene algo mas que decir
                    bool finish = true, finish_dialog = true;
                    float time = 0;
                    List<DialogTree> dialog = receptor.GetComponent<NPC>().dialog;
                    
                    for (int i = 0; i < dialog.Count; ++i)
                    {
                        for (int j = 0; j < dialog[i].dialog.Count; ++j)
                        {
                            if (!dialog[i].dialog[j].Said)
                            {
                                finish = false;

                                if (!dialog[i].Repeteable)
                                {
                                    if (index == i)
                                    {
                                        finish_dialog = false;
                                        time = (j - 1 < 0) ? 0 : dialog[i].dialog[j - 1].Time;
                                    }
                                }


                            }
                        }
                    }

                    if (finish) receptor.GetComponent<NPC>().SetMuted(); //si no tiene mas que decir le ignoramos
                    //

                    if (finish_dialog) //si a finalizado la rama de dialogo
                    {
                        QuestResolve q = receptor.gameObject.GetComponent<QuestResolve>();
                        if (q != null) //si tiene una mision para resolver
                        {
                            if (q.type == QuestResolveType.Speak) q.Resolve();
                        }

                        if (dialog[index].quest.code != 0) //comprobamos si tiene quest
                        {
                            if (!GameController.current.player.GetQuest(dialog[index].quest.code)) GameController.current.player.AddQuest(dialog[index].quest);
                        }

                        float t = dialog[index].dialog[dialog[index].dialog.Count - 1].Time + 1;

                        Invoke("FinishSpeak", t);
                        receptor.GetComponent<State_Speak>().Invoke("FinishSpeak", t);
                    }
                    else Invoke("ContinueSpeak", time + 0.5f);
                }
                else //en caso de que no tenga nada que decir
                {
                    FinishSpeak();
                    receptor.GetComponent<State_Speak>().FinishSpeak();
                }
            }
        }
    }

    public Controller GetReceptor()
    {
        return receptor;
    }

    public void ContinueSpeak()
    {
        speaking = false;
    }

    public void FinishSpeak()
    {
        controller.ChangeState<State_Idle>();
    }

    public override void OnMachineStateEnter(GameObject other)
    {
        base.OnMachineStateEnter(other);

        receptor = interactuable.GetComponent<Controller>();
    }

    public override void OnMachineStateExit(States new_state)
    {

    }
}
