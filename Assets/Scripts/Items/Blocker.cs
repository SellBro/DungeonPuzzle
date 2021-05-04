using Core;
using Pathfinding;
using UnityEngine;

namespace Items
{
    public class Blocker : MonoBehaviour
    {
        private SingleNodeBlocker blocker;

        private void Start()
        {
            blocker = GetComponent<SingleNodeBlocker>();
            blocker.manager = GameManager.Instance.blockManager;
            GameManager.Instance.AddObstacleToList(blocker);
            blocker.BlockAtCurrentPosition();
        }

        private void OnDestroy()
        {
            blocker.Unblock();
            Destroy(gameObject);
        }
    }
}
