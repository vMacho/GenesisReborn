using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestControllerUIList : MonoBehaviour
{
    public Text description;
    Quest quest;

    public void Initialize(Text d, Quest q)
    {
        description = d;
        quest = q;
        
        GetComponentInChildren<Text>().text = quest.text;
    }

    public void ViewDescription()
    {
        if (quest != null)
        {
            description.text = quest.description;
        }
    }


    
}