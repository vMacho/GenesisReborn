using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestReciever : MonoBehaviour
{
    public Quest quest;
    public QuestRecieverType type;
}

public enum QuestRecieverType
{ 
    Speak,
    ActionTrigger,
    GetItem
}
