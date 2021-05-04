using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Core
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private int levelToLoad = -1;

        public void LoadLevel(int index)
        {
            if(index < 0)
                Debug.LogError("Level Load Exception");

            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                if (index > PlayerPrefs.GetInt("LevelAt"))
                { 
                    PlayerPrefs.SetInt("LevelAt",index);
                }

                Cursor.visible = true;
                SceneManager.LoadScene(index);
            }
            
            
            
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
