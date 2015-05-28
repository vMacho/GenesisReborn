using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScreen : MonoBehaviour
{
    public Text text;

    int loading_progress = 0;
    AsyncOperation async;
    bool loaded = false;

	void Start () 
	{
        StartCoroutine(DisplayLoadingScreen(PlayerPrefs.GetString("LoadLevel")));
	}

    IEnumerator DisplayLoadingScreen(string level)
    {
        async = Application.LoadLevelAsync(level);
        async.allowSceneActivation = false;
        
        while (!async.isDone && async.progress < 0.9f)
        {
            loading_progress = (int)(async.progress * 100);
            text.text = "Loading... " + loading_progress + "%";
            yield return null;
        }

        loaded = true;

        yield return async;
    }

    void Update()
    {
        if (loaded)
        {
            text.text = "Loading... 100%";
            async.allowSceneActivation = true;
        }
    }
}
