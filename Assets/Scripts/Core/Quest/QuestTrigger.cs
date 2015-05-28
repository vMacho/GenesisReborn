using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestTrigger : MonoBehaviour
{
    public int code;
    public QuestTriggerType type;

    public void Activate()
    {
        switch (type)
        { 
            case QuestTriggerType.Desactive:
                this.gameObject.SetActive(false);
                break;
        }

    }
}

public enum QuestTriggerType
{ 
    Active,
    Desactive
}
