using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace ProjectStavitski.Core
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private int levelToLoad = -1;

        public void LoadLevel(int index)
        {
            if(index < 0)
                Debug.LogError("Level Load Exception");
            SceneManager.LoadScene(index);
        }

        public void QuitGame()
        {
            Debug.Log("Quitting");
            Application.Quit();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                LoadLevel(levelToLoad);
            }
        }
    }
}
