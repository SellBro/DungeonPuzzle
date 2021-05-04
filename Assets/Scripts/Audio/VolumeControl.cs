using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private MixerType type;
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider slider;

        
        private void Start()
        {
            switch (type) 
            { 
                case MixerType.AUDIO:
                    slider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("SoundVol") / 20);
                    mixer.SetFloat("SoundVol", PlayerPrefs.GetFloat("SoundVol")); 
                    break;
                case MixerType.MUSIC:
                    slider.value = Mathf.Pow(10, PlayerPrefs.GetFloat("MusicVol") / 20);
                    mixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol")); 
                    break;
            }
        }

        public void SetMusicLevel(float value)
        {
            value = Mathf.Log10(value) * 20;
            switch (type)
            {
                case MixerType.AUDIO:
                    mixer.SetFloat("SoundVol", value);
                    PlayerPrefs.SetFloat("SoundVol", value);
                    break;
                case MixerType.MUSIC:
                    mixer.SetFloat("MusicVol", value);
                    PlayerPrefs.SetFloat("MusicVol", value);
                    Debug.Log(PlayerPrefs.GetFloat("MusicVol"));
                    break;
            }
        }
    }

    enum MixerType
    {
        AUDIO,
        MUSIC,
        NONE
    }
}
