using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoard : MonoBehaviour
{
    [SerializeField]public AudioSource[] audios;
    public static AudioBoard instance;
    public bool isLocalBoard = false;

    private void Awake()
    {
        if(isLocalBoard) return;

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
                if(audio.isPlaying) return;
                
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

    public void ChangePitch(string audioName, float pitch)
    {
        foreach (AudioSource audio in audios)
        {
            if(audioName == audio.clip.name)
            {
                audio.pitch = pitch;
                return;
            }
        }
    }
}
