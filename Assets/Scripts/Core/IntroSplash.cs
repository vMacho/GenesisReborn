using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroSplash : MonoBehaviour
{
    bool canQuitSplashScreen = false;

	void Start () 
	{
        Invoke("PermitirQuitar", 2);
        Invoke("Quitar", 4);
	}
    
    void Update()
    {
        if (canQuitSplashScreen && (Input.GetAxis("Jump") > 0 || Input.GetAxis("Start") > 0)) Quitar();
    }

    public void Quitar(){ Application.LoadLevel("level_menu_start"); }

    public void PermitirQuitar()
    {
        canQuitSplashScreen = true;
    }
}
