using System;
using ProjectStavitski.Core;

namespace ProjectStavitski.Units
{
    public class EnemyUnit : Unit
    {
        public static Action<int> EnemyDie;
        
        private EnemyController _controller;

        private void Awake()
        {
            _controller = GetComponent<EnemyController>();
        }

        protected override void Die()
        {
            GameManager.Instance.RemoveUnitFromList(_controller);
            _controller.UnblockGridNode();
            Destroy(gameObject);
        }

        private void OnDisable()
        {
            EnemyDie?.Invoke(xpForKill);
        }
    }
}
