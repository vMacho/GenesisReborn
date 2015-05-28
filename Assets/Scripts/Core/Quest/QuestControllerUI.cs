using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestControllerUI : MonoBehaviour
{
    List<Quest> quests = new List<Quest>();

    void Start()
    {
        GameController.current.player.GetComponent<State_SearchingBag>().SetMode(2);

        EventSystem.current.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_Right";
        EventSystem.current.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_Right";

        quests = GameController.current.player.GetQuest();

        Text des = transform.FindChild("descrption").GetComponentInChildren<Text>();

        int i = 0;
        GameObject prev_item_slot = null, first_item_slot = null;
        foreach(Quest q in quests)
        {
            if (!q.IsDone())
            {
                GameObject item_slot = Instantiate(Resources.Load("UI/QuestList"), Vector3.zero, Quaternion.identity) as GameObject;
                item_slot.GetComponent<RectTransform>().SetParent(transform.FindChild("quests"));
                item_slot.gameObject.name = "Quest" + (i + 1);

                item_slot.GetComponent<RectTransform>().localPosition = new Vector3(-181, 58 - (28 * i), 0);
                item_slot.GetComponent<RectTransform>().localScale = Vector3.one;
                item_slot.GetComponent<QuestControllerUIList>().Initialize(des, q);

                Navigation item_nav = item_slot.GetComponent<Selectable>().navigation;

                if (prev_item_slot)
                {
                    Navigation pre_item_nav = prev_item_slot.GetComponent<Selectable>().navigation;
                    item_nav.selectOnUp = prev_item_slot.GetComponent<Selectable>();

                    pre_item_nav.selectOnDown = item_slot.GetComponent<Selectable>();
                    prev_item_slot.GetComponent<Selectable>().navigation = pre_item_nav;
                }
                else EventSystem.current.SetSelectedGameObject(item_slot); //es el primero

                item_slot.GetComponent<Selectable>().navigation = item_nav;
                item_slot.transform.SetAsFirstSibling();

                prev_item_slot = item_slot;

                if (first_item_slot == null)
                {
                    first_item_slot = item_slot;
                    item_slot.GetComponent<QuestControllerUIList>().ViewDescription();
                }

                i++;
            }
        }

        if (first_item_slot == null) //si no había
        {
            GameObject item_slot = Instantiate(Resources.Load("UI/QuestList"), Vector3.zero, Quaternion.identity) as GameObject;
            item_slot.GetComponent<RectTransform>().SetParent(transform.FindChild("quests"));
            item_slot.gameObject.name = "NoQuest";

            item_slot.GetComponent<RectTransform>().localPosition = new Vector3(-181, 58, 0);
            item_slot.GetComponent<RectTransform>().localScale = Vector3.one;

            item_slot.GetComponentInChildren<Text>().text = "No hay misiones pendientes";
        }
        else //para que pueda moverse del primero al último
        {
            Navigation pre_item_nav = prev_item_slot.GetComponent<Selectable>().navigation;
            Navigation item_nav = first_item_slot.GetComponent<Selectable>().navigation;


            item_nav.selectOnUp = prev_item_slot.GetComponent<Selectable>();
            first_item_slot.GetComponent<Selectable>().navigation = item_nav;            

            pre_item_nav.selectOnDown = first_item_slot.GetComponent<Selectable>();
            prev_item_slot.GetComponent<Selectable>().navigation = pre_item_nav;
        }
    }
}