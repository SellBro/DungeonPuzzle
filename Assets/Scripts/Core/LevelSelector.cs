using ProjectStavitski.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectStavitski.Core
{
    public class LevelSelector : MonoBehaviour
    {
        public Button[] levelButtons;

        private void Start()
        {
            int levelAt = PlayerPrefs.GetInt("LevelAt", 1);

            for (int i = 0; i < levelButtons.Length; ++i)
            {
                if (i + 1 > levelAt)
                {
                    levelButtons[i].GetComponent<LevelButton>().LockButton();
                }
            }
        }
    }
}
