using System;
using System.Collections;
using System.Collections.Generic;
using ProjectStavitski.Core;
using UnityEngine;

namespace ProjectStavitski.Utility
{
    class MusicController : MonoBehaviour
    {
        private static MusicController musicController = null;

        private void Start()
        {
            if (musicController == null)
            {
                musicController = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }
}
