using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;

public class lvl_menu_start_GM : MonoBehaviour 
{
    string[] levels = { "level_alpha" };
    string actual_level;

	// Use this for initialization
	void Start () 
    {
        for (int i = levels.Length - 1; i >= 0 - 1; ++i)
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
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void StartGame()
    {
        PlayerPrefs.SetString("LoadLevel", "level_alpha");
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
