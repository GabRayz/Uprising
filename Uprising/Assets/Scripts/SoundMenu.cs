using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMenu : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider master;
    public Slider effect;
    public Slider music;

    private void Start()
    {
        float masterVolume;
        mixer.GetFloat("MasterVolume", out masterVolume);
        master.value = masterVolume;

        if (effect != null)
        {
            float effectVolume;
            mixer.GetFloat("EffectVolume", out effectVolume);
            effect.value = effectVolume;
        }

        if (music != null)
        {
            float musicVolume;
            mixer.GetFloat("MusicVolume", out musicVolume);
            music.value = musicVolume;
        }
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", volume);
    }

    public void SetEffectVolume(float volume)
    {
        mixer.SetFloat("EffectVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", volume);
    }
}
