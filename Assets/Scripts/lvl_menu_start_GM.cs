using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;

public class lvl_menu_start_GM : MonoBehaviour 
{
    public Animator Credits, Options;
    public Toggle Mute, FullScreen;
    public Text ComboResolucion;

    string[] levels = { "level_one" };
    string actual_level;

	// Use this for initialization
	void Start () 
    {
        if (AudioManager.current == null) Instantiate(Resources.Load("Sounds/AudioManager")); //Si no hay un audioManager
        
        for (int i = levels.Length - 1; i >= 0; --i)
        {
            if (File.Exists(Application.persistentDataPath + "/" + levels[i] + "/savedGames.gd"))
            {
                Debug.Log("Ultimo level " + levels[i]);

                GameObject btn = GameObject.Find("Canvas/Menu/bt_continue");
                btn.SetActive(true);

                EventSystem.current.SetSelectedGameObject(btn);

                actual_level = levels[i];
                break;
            }
        }

        if (PlayerPrefs.GetInt("Muted") == 1) Mute.isOn = true;
        if (PlayerPrefs.GetInt("FullScreen") == 1) FullScreen.isOn = true;
        if (!PlayerPrefs.HasKey("Resolucion")) PlayerPrefs.SetString("Resolucion", Screen.width + " x " + Screen.height);

        ChangeResolution(PlayerPrefs.GetString("Resolucion"));

	}

    public void StartGame()
    {
        PlayerPrefs.SetString("LoadLevel", levels[0]);
        Application.LoadLevel("loading_screen");
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("continue", 1);
        PlayerPrefs.SetString("LoadLevel", actual_level);
        Application.LoadLevel("loading_screen");
    }

    public void OpenCredits()
    {
        Credits.gameObject.SetActive(true);
        Credits.SetBool("close", false);
    }

    public void CloseCredits()
    {
        Credits.SetBool("close", true);
    }

    public void OpenOptions()
    {
        Options.gameObject.SetActive(true);
        Options.SetBool("close", false);
    }

    public void CloseOptions()
    {
        Options.SetBool("close", true);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void ChangeMuted(bool val)
    {
        if (val) AudioManager.current.Muted();
        else AudioManager.current.UnMuted();
    }

    public void ChangeVolumen(float val)
    {
        AudioManager.current.SetVolumen(val);

        Mute.isOn = (val == 0);
    }

    public void ChangeFullScreen(bool val)
    {
        Screen.fullScreen = val;
        PlayerPrefs.SetInt("FullScreen", System.Convert.ToInt32(val));
    }
    
    public void ChangeResolution(string val)
    {
        string []r = val.Split(new char[]{'x'});

        Screen.SetResolution(int.Parse(r[0]), int.Parse(r[1]), (PlayerPrefs.GetInt("FullScreen") == 1) );
        PlayerPrefs.SetString("Resolucion", val);

        ComboResolucion.text = val;
    }
}
