using UnityEngine;
using UnityEngine.Audio;

namespace ProjectStavitski.Audio 
{
    public class VolumeLoadAdjustment : MonoBehaviour
    {
        [SerializeField] private AudioMixer soundMixer;
        [SerializeField] private AudioMixer musicMixer;
        private void Start()
        {
            soundMixer.SetFloat("SoundVol", PlayerPrefs.GetFloat("SoundVol")); 
            musicMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol")); 
        }
    }
}
