using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour 
{
    public static GameController current;

    public Player player = null;
    bool _gamepaused, _havetorestaureInventory;
    int kill_y = -30;

    GameObject menu_inventario, menu_quest, InfoNivel;
    Scrollbar healthbar;
    
    DayPos daylight;

    void Awake() 
    { 
        current = this;

        current.daylight = DayPos.dia;

        Invoke("HideInfo", 5);
    }

	void Start () 
	{
        current.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        current.healthbar = transform.Find("UI/GUI/healthbar").GetComponent<Scrollbar>();
        current.UpdateHealth();

        current.InfoNivel = transform.Find("UI/InfoNivel").gameObject;

        current.menu_inventario = null;

        if (PlayerPrefs.HasKey("continue"))
        {
            current.LoadGame();
            PlayerPrefs.DeleteKey("continue");
        }
        else
        {
            GameInfo.current = new GameInfo();
            Invoke("SaveGame", 1);
        }
	}

	void Update () 
	{
        if (current.player.transform.position.y <= current.kill_y) current.player.Kill();
	}

    public void UpdateHealth() { current.healthbar.size = (current.player.health / current.player.maxhealth); }

    public bool IsPaused() { return current._gamepaused; }

    public void SetDaylight(DayPos value) { current.daylight = value; }
    public DayPos GetDaylight() { return current.daylight; }

    public void SetGamePause(bool value)
    {
        GameObject menu = transform.Find("UI/Menu_Start").gameObject;
        current._gamepaused = value;

        if (value)
        {
            if (current.menu_inventario != null)
            {
                if (current.menu_inventario.GetComponent<CanvasGroup>().alpha == 1)
                {
                    current.menu_inventario.GetComponent<CanvasGroup>().alpha = 0;
                    current._havetorestaureInventory = true;
                }
                else current._havetorestaureInventory = false;
            }
            else current._havetorestaureInventory = false;

            menu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(GameObject.Find("btn_Resume"));
            EventSystem.current.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal";
            EventSystem.current.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical";
            
            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            menu.SetActive(false);
            if (current._havetorestaureInventory) current.menu_inventario.GetComponent<CanvasGroup>().alpha = 1;

            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void OpenInventory(Inventory bag, Controller controller, InventoryMode mode)
    {
        current.menu_inventario = Instantiate(Resources.Load("UI/Menu_Inventario"), Vector3.zero, Quaternion.identity) as GameObject;
        current.menu_inventario.transform.SetParent(transform.Find("UI"));
        current.menu_inventario.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 1);
        current.menu_inventario.GetComponent<InventoryController>().OpenInventory(bag, controller, mode);
        current.menu_inventario.GetComponent<RectTransform>().localPosition = new Vector3(267, 66, 0);
    }

    public void CloseInventory()
    {
        if (menu_inventario != null)
        {
            Destroy(current.menu_inventario);
            current.menu_inventario = null;
        }
    }

    public void OpenQuests()
    {
        current.menu_quest = Instantiate(Resources.Load("UI/Menu_Quest"), Vector3.zero, Quaternion.identity) as GameObject;
        current.menu_quest.transform.SetParent(transform.Find("UI"));
        current.menu_quest.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 1);
        current.menu_quest.GetComponent<RectTransform>().localPosition = new Vector3(267, 66, 0);
    }

    public void CloseQuests()
    {
        if (menu_quest != null)
        {
            Destroy(current.menu_quest);
            current.menu_quest = null;
        }
    }

    public void LoadCheckPoint()
    {
        SaveLoad.LoadCheckPoint();

        if (SaveLoad.checkpoint != null)
        {
            GameInfo.current = SaveLoad.checkpoint;
            GameInfo.current.LoadCheckPoint();
        }
        else SaveLoad.LoadGame(Application.loadedLevelName);
    }

    public void SaveGame()
    {
        GameInfo.current.player_post = new Vector3Serialized(current.player.gameObject.transform.position.x, current.player.gameObject.transform.position.y, current.player.gameObject.transform.position.z);
        GameInfo.current.player_inventory = new Inventory(current.player.GetInvetory().GetBag(), current.player.GetInvetory().OpenBag());

        SaveLoad.SaveGame();
    }

    public void LoadGame()
    {
        SaveLoad.LoadGame(Application.loadedLevelName);

        int partidas = SaveLoad.savedGames.Count;

        if (partidas > 0)
        {
            GameInfo.current = SaveLoad.savedGames[partidas - 1];
            GameInfo.current.LoadGame();
        }
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("continue", 1);
        PlayerPrefs.SetString("LoadLevel", Application.loadedLevelName);
        Application.LoadLevel("loading_screen");
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt("continue", 1);
        PlayerPrefs.SetString("LoadLevel", "level_menu_start");
        Application.LoadLevel("loading_screen");
    }

    public void HideInfo()
    {
        InfoNivel.gameObject.SetActive(false);
    }

	void OnGUI() 
	{
        if (current.player != null)
        {
            if (current._gamepaused) GUI.Label(new Rect(200, 10, 300, 20), "JUEGO PAUSADO");

            GUI.Label(new Rect(250, 10, 350, 20), "Time Controller:" + daylight.ToString());

            GUI.Label(new Rect(10, 10, 250, 20), "Velocidad " + current.player.GetRigidbody().velocity.ToString() + " Dirección " + current.player.GetDir());
            GUI.Label(new Rect(450, 10, 650, 20), "/\\ :" + current.player.GetButton(button_pad.Triangle) +
                                                 " 0 :" + current.player.GetButton(button_pad.Circle) +
                                                 " X :" + current.player.GetButton(button_pad.Cross) +
                                                 " []:" + current.player.GetButton(button_pad.Square));
        }
	}
}

public enum DayPos 
{
    dia,
    noche,
    atardecer,
    amanecer
}