using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;

public class lvl_menu_start_GM : MonoBehaviour 
{
    public Animator Credits, Options;
    public Toggle Mute, FullScreen;

    string[] levels = { "level_one" };
    string actual_level;

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
        if (!PlayerPrefs.HasKey("Resolucion"))
        {
            for (int i = 0; i < Screen.resolutions.Length; ++i)
            {
                if( Screen.resolutions[i].width == Screen.currentResolution.width && Screen.resolutions[i].height == Screen.currentResolution.height )
                {
                    PlayerPrefs.SetInt("Resolucion", i );
                    break;
                }
            }            
        }

        #if UNITY_EDITOR
        PlayerPrefs.SetInt("Resolucion", 0); //sólo hay una resolucion
        #endif

        Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("Resolucion")].width, Screen.resolutions[PlayerPrefs.GetInt("Resolucion")].height, (PlayerPrefs.GetInt("FullScreen") == 1));
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
        Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("Resolucion")].width, Screen.resolutions[PlayerPrefs.GetInt("Resolucion")].height, val);
        PlayerPrefs.SetInt("FullScreen", System.Convert.ToInt32(val));
    }
}
