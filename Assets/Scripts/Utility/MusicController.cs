using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    class MusicController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;
        
        private static MusicController musicController = null;

        private void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0 && musicController != null)
            {
                Destroy(musicController.gameObject);
                musicController = null;
            }
            
            if (musicController == null)
            {
                musicController = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            if (SceneManager.GetActiveScene().buildIndex != 0)
                DontDestroyOnLoad(gameObject);

            GetComponent<AudioSource>().clip = SceneManager.GetActiveScene().buildIndex == 0 ? clips[0] : clips[1];
            GetComponent<AudioSource>().Play();
        }
    }
}
