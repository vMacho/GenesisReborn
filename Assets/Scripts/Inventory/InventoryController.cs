using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour
{
    Sprite default_sprite;
    Inventory _lastBag;
    Controller _lastController;
    InventoryMode _lastMode;
    
    public void OpenInventory(Inventory bag, Controller controller, InventoryMode mode)
    {
        _lastBag = bag;
        _lastController = controller;
        _lastMode = mode;

        EventSystem.current.SetSelectedGameObject(GameObject.Find("Slot_Head"));
        EventSystem.current.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_Right";
        EventSystem.current.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_Right";

        switch (mode)
        {
            case InventoryMode.self:
                Open_armor(controller);
                Open_Bag(bag, controller);
                break;
        }

        GameController.current.player.GetComponent<State_SearchingBag>().SetMode(1);
    }

    public void RefreshInventory()
    {
        switch (_lastMode)
        {
            case InventoryMode.self:
                Open_armor(_lastController);

                foreach (InventoryItemController g in GameObject.Find("Items").GetComponentsInChildren<InventoryItemController>()) Destroy(g.gameObject);
                Open_Bag(_lastBag, _lastController);
                break;
        }

        EventSystem.current.SetSelectedGameObject(GameObject.Find("Slot_Boots"));
    }

    void Open_armor(Controller controller)
    {
        /**** PINTAMOS LA ARMADURA ********/
        if (controller.GetArmor(Armor_Slot.Head).obj != "")
        {
            GameObject slot = GameObject.Find("Slot_Head");
            Item_Armor item = controller.GetArmor(Armor_Slot.Head);
            if(item.img != "") slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + item.img);
            slot.GetComponent<InventoryItemController>().SetItem(item);
            slot.GetComponent<InventoryItemController>().SetController(controller);
        }


        if (controller.GetArmor(Armor_Slot.Chest).obj != "")
        {
            GameObject slot = GameObject.Find("Slot_Chest");
            Item_Armor item = controller.GetArmor(Armor_Slot.Chest);
            if (item.img != "") slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + item.img);
            slot.GetComponent<InventoryItemController>().SetItem(item);
            slot.GetComponent<InventoryItemController>().SetController(controller);

        }
        
        if (controller.GetArmor(Armor_Slot.Arms).obj != "")
        {
            GameObject slot = GameObject.Find("Slot_Arms");
            Item_Armor item = controller.GetArmor(Armor_Slot.Arms);
            if (item.img != "") slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + item.img);
            slot.GetComponent<InventoryItemController>().SetItem(item);
            slot.GetComponent<InventoryItemController>().SetController(controller);
        }


        if (controller.GetArmor(Armor_Slot.Legs).obj != "")
        {
            GameObject slot = GameObject.Find("Slot_Legs");
            Item_Armor item = controller.GetArmor(Armor_Slot.Legs);
            if (item.img != "") slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + item.img);
            slot.GetComponent<InventoryItemController>().SetItem(item);
            slot.GetComponent<InventoryItemController>().SetController(controller);
        }


        if (controller.GetArmor(Armor_Slot.Boots).obj != "")
        {
            GameObject slot = GameObject.Find("Slot_Boots");
            Item_Armor item = controller.GetArmor(Armor_Slot.Boots);
            if (item.img != "") slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + item.img);
            slot.GetComponent<InventoryItemController>().SetItem(item);
            slot.GetComponent<InventoryItemController>().SetController(controller);
        }

        if (controller.GetInvetory().Size() > 0)
        {
            GameObject slot = GameObject.Find("Slot_Bag");
            if (controller.GetInvetory().GetImage() != "") slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + controller.GetInvetory().GetImage());
        }

        if (controller.HasWeapon())
        {
            GameObject slot = GameObject.Find("Slot_Weapon");
            if (controller.GetWeapon().img != "") slot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + controller.GetWeapon().img);
        }
        /**********************************/
    }

    public void Open_Bag(Inventory bag, Controller controller)
    {
        /**** PINTAMOS LOS OBJETOS ********/
        List<Item> items = bag.OpenBag();
        RectTransform Panel = GameObject.Find("Items").GetComponent<RectTransform>();

        int y = 0;
        GameObject prev_item_slot = null;
        for (int i = 0; i < bag.Size(); ++i)
        {
            GameObject item_slot = Instantiate(Resources.Load("UI/Invetory_Item"), Vector3.zero, Quaternion.identity) as GameObject;
            item_slot.GetComponent<RectTransform>().SetParent(Panel);
            item_slot.gameObject.name = "BagSlot" + (i + 1);

            item_slot.GetComponent<RectTransform>().localPosition = new Vector3(-170 + (40 * i), 90 + (40 * y), 0);
            item_slot.GetComponent<RectTransform>().localScale = Vector3.one;

            if (((i + 1) % 5) == 0) ++y;

            Navigation item_nav = item_slot.GetComponent<Selectable>().navigation;

            item_nav.selectOnUp = GameObject.Find("Slot_Boots").GetComponent<Selectable>();

            if (prev_item_slot)
            {
                Navigation pre_item_nav = prev_item_slot.GetComponent<Selectable>().navigation;
                item_nav.selectOnLeft = prev_item_slot.GetComponent<Selectable>();

                pre_item_nav.selectOnRight = item_slot.GetComponent<Selectable>();
                prev_item_slot.GetComponent<Selectable>().navigation = pre_item_nav;
            }
            else
            {
                Selectable slot_1 = GameObject.Find("Slot_Weapon").GetComponent<Selectable>();
                Selectable slot_2 = GameObject.Find("Slot_Boots").GetComponent<Selectable>();

                Navigation slot1_nav = slot_1.navigation;
                Navigation slot2_nav = slot_2.navigation;

                slot1_nav.selectOnDown = slot2_nav.selectOnDown = item_slot.GetComponent<Selectable>();

                slot_1.navigation = slot1_nav;
                slot_2.navigation = slot2_nav;
            }

            item_slot.GetComponent<Selectable>().navigation = item_nav;

            item_slot.transform.SetAsFirstSibling();

            if (items.Count > i)
            {
                item_slot.GetComponent<InventoryItemController>().SetItem(items[i]);
                item_slot.GetComponent<InventoryItemController>().SetController(controller);
            }
            prev_item_slot = item_slot;
        }
        /*******************************************************/
    }
}

public enum InventoryMode
{
    self
}

public enum ItemType
{
    Heal,
    Armor,
    Weapon,
    Bag
}