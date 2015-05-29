using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;

public class lvl_menu_start_GM : MonoBehaviour 
{
    public AudioClip BackgroundMusic;

    string[] levels = { "level_one" };
    string actual_level;

	// Use this for initialization
	void Start () 
    {
        if (AudioManager.current == null) Instantiate(Resources.Load("Sounds/AudioManager")); //Si no hay un audioManager
        if (!AudioManager.current.IsPlaying(BackgroundMusic)) AudioManager.current.Play(BackgroundMusic); //Si no se esta reproduciendo hazlo
        
        for (int i = levels.Length - 1; i >= 0; --i)
        {
            if (File.Exists(Application.persistentDataPath + "/" + levels[i] + "/savedGames.gd"))
            {
                Debug.Log("Ultimo level " + levels[i]);

                GameObject btn = GameObject.Find("Canvas/Panel/bt_continue");
                btn.SetActive(true);

                EventSystem.current.SetSelectedGameObject(btn);

                actual_level = levels[i];
                break;
            }
        }
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

    public void EndGame()
    {
        Application.Quit();
    }
}
