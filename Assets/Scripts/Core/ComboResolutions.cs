using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboResolutions : MonoBehaviour
{
    public GameObject comboOption;
    public GameObject Panel, ComboButton;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        for (int i = resolutions.Length - 1; i > resolutions.Length - 8 && i >= 0; --i)
        {
            GameObject g = Instantiate(comboOption) as GameObject;

            g.GetComponentInChildren<Text>().text = resolutions[i].width + " x " + resolutions[i].height;
            
            int index = i;
            g.GetComponentInChildren<Button>().onClick.AddListener(
                () => { SetResolution(index); }
            );

            g.transform.SetParent(Panel.transform);
            g.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }

        ComboButton.GetComponentInChildren<Text>().text = resolutions[PlayerPrefs.GetInt("Resolucion")].width + " x " + resolutions[PlayerPrefs.GetInt("Resolucion")].height;
    }

    public void SetResolution(int val)
    {
        Screen.SetResolution(resolutions[val].width, resolutions[val].height, (PlayerPrefs.GetInt("FullScreen") == 1));
        PlayerPrefs.SetInt("Resolucion", val);

        ComboButton.GetComponentInChildren<Text>().text = resolutions[val].width + " x " + resolutions[val].height;

        Panel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(ComboButton);
    }

    public void OpenResolutions()
    {
        EventSystem.current.SetSelectedGameObject(Panel.transform.GetChild(0).gameObject);
        Panel.SetActive(true);
    }
}