using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Uprising.Items;

namespace Uprising.Players
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        public AudioMixerGroup mixer;

        // Start is called before the first frame update
        void Awake()
        {
            foreach(Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.outputAudioMixerGroup = mixer;
            }
        }

        public void PlaySound(string name)
        {
            Sound sound = sounds.ToList().Find(s => s.name == name);
            if (sound != null)
                sound.source.Play();
            else
                Debug.LogWarning("The specified sound has not been found : " + name);
        }
    }
}