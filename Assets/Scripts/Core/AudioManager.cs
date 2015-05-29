//AudioManager
/******************** VICTOR MACHO **************/
//Controlador del audio del juego, se carga en la primera escena y perdura en el resto de escenas
//En el resto de las escenas se deberá comprobar si existe y en caso contrario instanciarlo
/***********************************************/

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;
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

            current = this;
        }

        DontDestroyOnLoad( gameObject ); //Para perdurar por las escenas
    }
    
    public void Play(AudioClip clip) //Escuchamos la canción de fondo
    {
        if (Audio)
        {
            Audio.clip = clip;

            Audio.Play();
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

    public void SetVolumen( float val )
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
}