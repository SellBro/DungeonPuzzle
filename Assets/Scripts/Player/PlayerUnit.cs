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


        private int split;
        
        protected override void Start()
        {
            base.Start();
            split = (int)(_health / healthUI.Length);
        }

        private void Update()
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
