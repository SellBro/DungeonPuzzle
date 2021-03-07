using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectStavitski.UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite lockedSprite;

        private Button _button;
        private Image _image;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        }

        public void LoadLevel(int level = 0)
        {
            SceneManager.LoadScene(level);
        }
        
        public void LockButton()
        {
            _button.interactable = false;
            _image.sprite = lockedSprite;
        }
        public void UnlockButton()
        {
            _button.interactable = true;
            _image.sprite = activeSprite;
        }
    }
}
