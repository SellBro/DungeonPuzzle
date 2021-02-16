using System;
using System.Collections;
using System.Collections.Generic;
using ProjectStavitski.Player;
using UnityEngine;

namespace ProjectStavitski.Items
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private ItemConfig config = null;
        [SerializeField] private int healthToRestore = 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (config != null)
                subject.GetComponent<PlayerMovementController>().EquipItem(config);

            if(healthToRestore > 0)
                subject.GetComponent<PlayerUnit>().Heal(healthToRestore);
        }
    }
}
