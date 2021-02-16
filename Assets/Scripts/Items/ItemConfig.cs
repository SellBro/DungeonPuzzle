using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectStavitski.Items
{
   [CreateAssetMenu(fileName = "ItemConfig", menuName = "ItemConfig/Make new Weapon", order = 0)]
   public class ItemConfig : ScriptableObject
   {
      public int damage = 0; 
      public int armour = 0; 
      public bool canCutTrees = false; 
      public bool canBreakWalls = false;
      
      [SerializeField] GameObject itemGameObject = null;
      [SerializeField] private Sprite itemSprite = null; 
      

      public void EquipNewItem(Transform pos,Image image, ItemConfig oldItem)
      {
         DropOldItem(pos, oldItem);

         image.sprite = itemSprite;
      }

      private void DropOldItem(Transform pos, ItemConfig oldItem)
      {
         Instantiate(oldItem.itemGameObject, pos);
      }
   } 
}
