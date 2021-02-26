using System;
using ProjectStavitski.Units;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectStavitski.Player
{
    public class PlayerUnit : Unit
    {
        [SerializeField] private Image[] healthUI;
        [SerializeField] private Sprite[] healthSprites;


        private PlayerMovementController _playerMovementController;

        private int split;

        private void Awake()
        {
            _playerMovementController = GetComponent<PlayerMovementController>();
        }

        protected override void Start()
        {
            base.Start();
            split = (int)(_health / healthUI.Length);
        }

        public override void TakeDamage(int amount)
        {
            if (_playerMovementController.GetCurrentItemArmour() > 0)
            {
                _playerMovementController.DecreaseCurrentItemArmour(amount);
            }
            else
            {
                base.TakeDamage(amount);
                UpdateHealthBar();
            }
        }

        public override void Heal(int amount)
        {
            base.Heal(amount);
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            for (int i = 1; i <= healthUI.Length; ++i)
            {
                if (_health >= split * i)
                { 
                    healthUI[i - 1].sprite = healthSprites[healthSprites.Length - 1];
                }
                else if (_health >= split * i - 1)
                {
                    healthUI[i - 1].sprite = healthSprites[healthSprites.Length - 2];
                }
                else
                {
                    healthUI[i - 1].sprite = healthSprites[0];
                }
            }
        }

        public override int GetDamage()
        {
            return damage;
        }
    }
}
