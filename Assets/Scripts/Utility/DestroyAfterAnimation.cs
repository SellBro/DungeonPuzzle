using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectStavitski.Utility 
{
    public class DestroyAfterAnimation : MonoBehaviour
    {
        [SerializeField] private float animationDuration = 0.15f;

        private void Start()
        {
            Destroy(gameObject, animationDuration);
        }
    }
}
