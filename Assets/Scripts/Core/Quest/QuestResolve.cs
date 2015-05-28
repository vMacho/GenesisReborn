using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestResolve : MonoBehaviour
{
    public Quest quest;
    public QuestResolveType type;

    public void Resolve()
    {
        GameController.current.player.FinishQuest(quest);

        Object[] objects = FindObjectsOfType(typeof(QuestTrigger));
        foreach (QuestTrigger q in objects) if (q.code == quest.code) q.Activate();

        Destroy(this);
    }
}

public enum QuestResolveType
{ 
    Speak,
    ActionTrigger,
    GetItem
}
