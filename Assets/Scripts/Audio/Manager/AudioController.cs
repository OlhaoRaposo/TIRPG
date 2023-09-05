using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField]private AudioMixer mixer;
    [SerializeField]private Slider master, bgm, sfx;

    private void Start() //Seta todos os valores de audio para os valores salvos quando o código é iniciado.
    {
        /*master.value = PlayerPrefs.GetFloat("masterVol", master.value);
        bgm.value = PlayerPrefs.GetFloat("bgmVol", bgm.value);
        sfx.value = PlayerPrefs.GetFloat("sfxVol", sfx.value);*/
    }
    public void MasterVolumeChange() //O "BGMVolumeChange" e o "SFXVolumeChange" seguem a mesma lógica.
    {
        if(master.value != -20)
        {
            mixer.SetFloat("masterVol", master.value); //Seta o valor do audio para o valor do slider.
            PlayerPrefs.SetFloat("masterVol", master.value); //Salva o último valor de audio setado pelo usuário.
        }
        else //Caso o slider esteja com seu valor mínimo, ele seta o valor da caixa de som diretamente para o -80, serve para não causar um efeito esquisito ao mexer o slider :).
        {
            mixer.SetFloat("masterVol", -80);
            PlayerPrefs.SetFloat("masterVol", -80);
        }
    }
    public void BGMVolumeChange()
    {
        if(bgm.value != -20)
        {
            mixer.SetFloat("bgmVol", bgm.value);
            PlayerPrefs.SetFloat("bgmVol", bgm.value);
        }
        else
        {
            mixer.SetFloat("bgmVol", -80);
            PlayerPrefs.SetFloat("bgmVol", -80);
        }
    }
    public void SFXVolumeChange()
    {
        if(sfx.value != -20)
        {
            mixer.SetFloat("sfxVol", sfx.value);
            PlayerPrefs.SetFloat("sfxVol", sfx.value);
        }
        else
        {
            mixer.SetFloat("sfxVol", -80);
            PlayerPrefs.SetFloat("sfxVol", -80);
        }
    }
}
