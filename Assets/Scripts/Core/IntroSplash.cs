using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroSplash : MonoBehaviour
{
    bool canQuitSplashScreen = false;

	void Start () 
	{
        if (AudioManager.current == null) Instantiate(Resources.Load("Sounds/AudioManager")); //Si no hay un audioManager

        Invoke("PermitirQuitar", 1);
        Invoke("Quitar", 4);
	}
    
    void Update()
    {
        if (canQuitSplashScreen && (Input.GetAxis("Jump") > 0 || Input.GetAxis("Start") > 0 || Input.touchCount > 0)) Quitar();
    }

    public void Quitar(){ Application.LoadLevel("level_menu_start"); }

    public void PermitirQuitar()
    {
        canQuitSplashScreen = true;
    }
}
