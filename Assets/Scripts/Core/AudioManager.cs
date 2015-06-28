//AudioManager
/******************** VICTOR MACHO **************/
//Controlador del audio del juego, se carga en la primera escena y perdura en el resto de escenas
//En el resto de las escenas se deberá comprobar si existe y en caso contrario instanciarlo
/***********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    [Header("Lista Canciones")]
    public int duracionAviso = 4;
    public List<Cancion> PlayList = new List<Cancion>();
    int current_song = -1;
    string sonando = "";

    GameObject aviso;
    AudioSource Audio;
    float prev_volumen;

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Audio = gameObject.AddComponent<AudioSource>();

            if (PlayerPrefs.GetInt("Muted") == 1) Audio.volume = 0; //si esta muteado el sonido
            else if (!PlayerPrefs.HasKey("Volumen")) Audio.volume = 1; //Primera vez que ejecuta el juego
            else Audio.volume = PlayerPrefs.GetFloat("Volumen");

            prev_volumen = Audio.volume;

            Audio.loop = false; //Loop del background

            current = this;
        }

        Shuffle(PlayList); //Reordenamos la playlist

        DontDestroyOnLoad(gameObject); //Para perdurar por las escenas
    }

    void Update()
    {
        if (!Audio.isPlaying && PlayList.Count > 0)
        {
            Next();
            //MostrarAviso();
        }

        //Depuracion
        if (Input.GetKeyDown(KeyCode.N)) Next();
    }

    public void Play(AudioClip clip) //Escuchamos la canción de fondo (una)
    {
        if (Audio)
        {
            Audio.clip = clip;

            Audio.Play();
        }
    }

    public void Next() //Escuchamos la canción siguiente de fondo (playlist)
    {
        //QuitarAviso();

        current_song = (current_song + 1 == PlayList.Count) ? 0 : current_song + 1;

        Audio.clip = PlayList[current_song].Song;

        sonando = PlayList[current_song].Song.name + " de " + PlayList[current_song].Grupo;
        Debug.Log("Sonando " + sonando);

        Audio.Play();
    }

    public void Prev() //Escuchamos la canción anterior de fondo (playlist)
    {
        //QuitarAviso();

        current_song = (current_song - 1 < 0) ? PlayList.Count - 1 : current_song - 1;

        Audio.clip = PlayList[current_song].Song;

        sonando = PlayList[current_song].Song.name + " de " + PlayList[current_song].Grupo;
        Debug.Log("Sonando " + sonando);

        Audio.Play();
    }

    public static void Shuffle<T>(List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public void Cancel() //Paramos la reproducción
    {
        Audio.Stop();
    }

    public bool IsPlaying(AudioClip clip) //Si se está escuchando el clip se devuelve true
    {
        if (Audio.clip == clip) return Audio.isPlaying;
        else return false;
    }

    public float GetVolumen() { return Audio.volume; }

    public void SetVolumen(float val)
    {
        PlayerPrefs.SetFloat("Volumen", val);
        Audio.volume = prev_volumen = val;

        if (val == 0) PlayerPrefs.SetInt("Muted", 1);
        else PlayerPrefs.SetInt("Muted", 0);
    }

    public void Muted()
    {
        PlayerPrefs.SetFloat("Volumen", 0);
        Audio.volume = 0;

        PlayerPrefs.SetInt("Muted", 1);
    }

    public void UnMuted()
    {
        PlayerPrefs.SetFloat("Volumen", prev_volumen);
        Audio.volume = prev_volumen;

        PlayerPrefs.SetInt("Muted", 0);
    }

    /*void MostrarAviso()
    {
        aviso = Instantiate(Resources.Load("PlayList/AvisoCancion")) as GameObject;
        aviso.GetComponentInChildren<Text>().text = "Sonando " + sonando;
        aviso.transform.SetParent(this.transform);

        Invoke("QuitarAviso", duracionAviso);
    }

    public void QuitarAviso()
    {
        Destroy(aviso);
    }*/

    public string Sonando()
    {
        return sonando;
    }
}

[System.Serializable]
public struct Cancion
{
    public AudioClip Song;
    public string Grupo;
}