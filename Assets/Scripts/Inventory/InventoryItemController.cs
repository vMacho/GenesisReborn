using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour 
{
    Controller controller;
    GameObject _menu;
    Item _item = null;
    bool empty = true;

    public void SetItem(Item i)
    {
        empty = false;
        _item = i;

        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + _item.img);
    }

    public void SetController(Controller c) { controller = c; }

	void Start () 
    {
        _menu = transform.FindChild("MenuItem").gameObject;
	}

    public void OpenMenu()
    {
        if (!empty && !_menu.activeSelf)
        {
            _menu.SetActive(true);

            GameObject prev = null;
            foreach (RectTransform rect in gameObject.GetComponentsInChildren<RectTransform>())
            {
                if (rect.gameObject.GetComponent<Button>())
                {
                    Navigation nav = rect.gameObject.GetComponent<Button>().navigation;

                    if (prev)
                    {
                        nav.selectOnUp = prev.GetComponent<Button>();

                        Navigation pre_nav = prev.GetComponent<Button>().navigation;
                        pre_nav.selectOnDown = rect.gameObject.GetComponent<Button>();
                        prev.GetComponent<Button>().navigation = pre_nav;
                    }
                    else EventSystem.current.SetSelectedGameObject(rect.gameObject);

                    rect.gameObject.GetComponent<Button>().navigation = nav;
                    prev = rect.gameObject;
                }
            }
        }
    }

    public void CloseMenu()
    {
        if (_menu.activeSelf)
        {
            _menu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void UseItem()
    {
        if (controller.UseItem(_item))
        {
            CloseMenu();

            GetComponentInParent<InventoryController>().RefreshInventory();
        }
    }

    public void DropItem()
    {
        if (controller.DropItem(_item))
        {
            CloseMenu();

            GetComponentInParent<InventoryController>().RefreshInventory();
        }
    }

    public void ExamineItem()
    {
        controller.ExamineItem(_item);
        CloseMenu();
    }
}
