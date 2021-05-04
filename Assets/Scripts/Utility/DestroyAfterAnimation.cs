using UnityEngine;

namespace Utility 
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
