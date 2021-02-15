using ProjectStavitski.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectStavitski.Player
{
    public class PlayerUnit : Unit
    {
        [Header("Player Unit Settings")]
        public int level = 1;
        public int xP;
        public int xPToNextLevel = 100;
        public int additionalDamage = 0;
        public int additionalHealth = 0;
        public int additionalArmour = 0;

        private void OnEnable()
        {
            EnemyUnit.EnemyDie += AddXP;
        }
        
        private void OnDisable()
        {
            EnemyUnit.EnemyDie -= AddXP;
        }

        public void AddXP(int amount)
        {
            xP += amount;
            if (xP >= xPToNextLevel)
            {
                int temp = xP - xPToNextLevel;
                ++level;
                xP = temp;
                xPToNextLevel += 100;
            }
        }

        public override int GetDamage()
        {
            return damage + additionalDamage;
        }
    }
}
