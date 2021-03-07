using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectStavitski.Player
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUI;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Cursor.visible)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                pauseUI.SetActive(!pauseUI.activeSelf);
            }
        }
    }
}
