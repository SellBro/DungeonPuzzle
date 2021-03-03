using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectStavitski.Player
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUI;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseUI.SetActive(!pauseUI.activeSelf);
            }
        }
    }
}
