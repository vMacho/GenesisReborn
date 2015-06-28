using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour 
{
    public static GameController current;
    
    public Player player = null;
    bool _gamepaused, _havetorestaureInventory;
    int kill_y = -30;

    public GameObject InfoNivel, MobileControl, menu_start;
    GameObject menu_quest, menu_inventario, menu_inventario_lastfocus;
    public Scrollbar healthbar;
    
    DayPos daylight;

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            current = this;

            current.daylight = DayPos.dia;

            Invoke("HideInfo", 5);
        }

        #if MOBILE_INPUT
        MobileControl.SetActive(true);
        #endif
    }

	void Start () 
	{
        #if MOBILE_INPUT
        MobileControl.GetComponentInChildren<Joystick>().Initialize();
        #endif

        current.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        current.UpdateHealth();

        current.menu_inventario = null;

        if (PlayerPrefs.HasKey("continue"))
        {
            current.LoadGame();
            PlayerPrefs.DeleteKey("continue");
        }
        else
        {
            GameInfo.current = new GameInfo();
            //Invoke("SaveGame", 1);
        }

        if (AudioManager.current == null) Instantiate(Resources.Load("Sounds/AudioManager")); //Si no hay un audioManager

        AudioManager.current.Cancel();
	}

	void Update () 
	{
        if (current.player.transform.position.y <= current.kill_y) current.player.Kill();
	}

    public void UpdateHealth() { healthbar.size = (current.player.health / current.player.maxhealth); }

    public bool IsPaused() { return _gamepaused; }

    public void SetDaylight(DayPos value) { daylight = value; }
    public DayPos GetDaylight() { return daylight; }

    public void SetGamePause(bool value)
    {
        _gamepaused = value;

        if (value)
        {
            if (menu_inventario != null)
            {
                if (menu_inventario.GetComponent<CanvasGroup>().alpha == 1)
                {
                    menu_inventario_lastfocus = EventSystem.current.currentSelectedGameObject;
                    menu_inventario.GetComponent<CanvasGroup>().alpha = 0;
                    _havetorestaureInventory = true;
                }
                else _havetorestaureInventory = false;
            }
            else _havetorestaureInventory = false;

            menu_start.SetActive(true);

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
            menu_start.SetActive(false);
            if (_havetorestaureInventory)
            {
                menu_inventario.GetComponent<CanvasGroup>().alpha = 1;

                EventSystem.current.SetSelectedGameObject(menu_inventario_lastfocus);
                EventSystem.current.GetComponent<StandaloneInputModule>().horizontalAxis = "Horizontal_Right";
                EventSystem.current.GetComponent<StandaloneInputModule>().verticalAxis = "Vertical_Right";
            }

            Invoke("ResumeObjects", 0.1f); //Volvemos en el siguiente frame para que no salte al aceptar el boton volver
        }
    }

    public void ResumeObjects()
    {
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
        {
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OpenInventory(Inventory bag, Controller controller, InventoryMode mode)
    {
        menu_inventario = Instantiate(Resources.Load("UI/Menu_Inventario"), Vector3.zero, Quaternion.identity) as GameObject;
        menu_inventario.transform.SetParent(transform.Find("UI"));
        menu_inventario.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        menu_inventario.GetComponent<InventoryController>().OpenInventory(bag, controller, mode);
        menu_inventario.GetComponent<RectTransform>().localPosition = new Vector3(267, 66, 0);
    }

    public void CloseInventory()
    {
        if (menu_inventario != null)
        {
            Destroy(menu_inventario);
            menu_inventario = null;
        }
    }

    public void OpenQuests()
    {
        menu_quest = Instantiate(Resources.Load("UI/Menu_Quest"), Vector3.zero, Quaternion.identity) as GameObject;
        menu_quest.transform.SetParent(transform.Find("UI"));
        menu_quest.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 1);
        menu_quest.GetComponent<RectTransform>().localPosition = new Vector3(267, 66, 0);
    }

    public void CloseQuests()
    {
        if (menu_quest != null)
        {
            Destroy(menu_quest);
            menu_quest = null;
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
        GameInfo.current.player_post = new Vector3Serialized(player.gameObject.transform.position.x, player.gameObject.transform.position.y, player.gameObject.transform.position.z);
        GameInfo.current.player_inventory = new Inventory(player.GetInvetory().GetBag(), player.GetInvetory().OpenBag());

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

	void OnGUI() //DEPURACION
	{
        if (player != null)
        {
            GUI.Label(new Rect(250, 10, 350, 20), "Time Controller:" + daylight.ToString());

            GUI.Label(new Rect(10, 10, 250, 20), "Velocidad " + player.GetRigidbody().velocity.ToString() + " Dirección " + player.GetDir());
            GUI.Label(new Rect(450, 10, 650, 20), "/\\ :" + player.GetButton(button_pad.Triangle) +
                                                 " 0 :" + player.GetButton(button_pad.Circle) +
                                                 " X :" + player.GetButton(button_pad.Cross) +
                                                 " []:" + player.GetButton(button_pad.Square));
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