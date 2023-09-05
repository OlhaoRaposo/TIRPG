using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoard : MonoBehaviour
{
    [SerializeField]public AudioSource[] audios;
    public static AudioBoard instance;

    private void Awake()
    {
        audios = GameObject.FindObjectsOfType<AudioSource>();
        if (instance == null)
        {
            instance = this;
        }else
            Destroy(this.gameObject);
    }

    public void PlayAudio(string audioName)
    {
        foreach (AudioSource audio in audios)
        {
            if(audioName == audio.clip.name)
            {
                audio.Play();
                return;
            }
        }
    }

    public void StopAudio(string audioName)
    {
        foreach (AudioSource audio in audios)
        {
            if(audioName == audio.clip.name)
            {
                audio.Stop();
                return;
            }
        }
    }

    public void ResumeAudio(string audioName)
    {
        foreach (AudioSource audio in audios)
        {
            if(audioName == audio.clip.name)
            {
                audio.UnPause();
                return;
            }
        }
    }

    public void PauseAudio(string audioName)
    {
        foreach (AudioSource audio in audios)
        {
            if(audioName == audio.clip.name)
            {
                audio.Pause();
                return;
            }
        }
    }
}
