using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConversationController : MonoBehaviour 
{
    float startDelay = 0f, typeDelay = 0.005f, lifetime, actual_lifetime = 0;
    public AudioClip sound;
    bool dead, finished = false;
    string text;
    CanvasGroup canvas;
    Text textcomponent;
    AudioSource audioSource;

    public string GetText() { return text; }
    public void SetTime(float time) { lifetime = time; actual_lifetime = 0; }

	public void Initialize ( string txt, float time) 
    {
        lifetime = time;

        canvas = transform.GetChild(0).GetComponent<CanvasGroup>();

        text = txt;
        textcomponent = GetComponentInChildren<Text>();
        audioSource = GetComponentInChildren<AudioSource>();
        StartCoroutine("TypeIn");        
	}

    void Update() 
    {
        if (!GameController.current.IsPaused())
        {
            if (transform.eulerAngles != Vector3.zero) transform.eulerAngles = Vector3.zero; //que siempre mire a camara

            actual_lifetime += Time.deltaTime;

            if (dead)
            {
                canvas.alpha -= Time.deltaTime;

                if (canvas.alpha <= 0) ShutUp();
            }
            else if (finished && actual_lifetime >= lifetime && lifetime != 0) dead = true;
        }
	}

    public void ShutUp()
    {
        audioSource.Stop();

        foreach (ConversationController c in GetComponentsInChildren<ConversationController>())
        {
            c.transform.SetParent(transform.parent);

            Vector3 pos = transform.parent.position;

            if (transform.parent.parent == null) pos += new Vector3(0.2f, 1f, 0);
            else pos += new Vector3(2.2f, 1, 0);

            c.transform.position = pos;
        }

        Destroy(this.gameObject);
    }

    public IEnumerator TypeIn()
    {
        yield return new WaitForSeconds(startDelay);

        if (!audioSource.isPlaying) audioSource.PlayOneShot(sound);

        for (int i = 0; i <= text.Length; ++i)
        {
            textcomponent.text = text.Substring(0, i);

            yield return new WaitForSeconds(typeDelay);
        }

        audioSource.Stop();
        finished = true;
    }

    public IEnumerator TypeOff()
    {

        for (int i = text.Length; i >= 0; --i)
        {
            textcomponent.text = text.Substring(0, i);

            yield return new WaitForSeconds(typeDelay);
        }

        finished = true;
    }
}
