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
      

      public void EquipNewItem(Image image)
      { 
         itemSprite = itemGameObject.GetComponent<SpriteRenderer>().sprite;
         image.sprite = itemSprite;
      }

      public void DropThisItem(Vector3 pos)
      {
         GameObject item = Instantiate(itemGameObject, pos, Quaternion.identity);
         item.GetComponent<BoxCollider2D>().isTrigger = true;
      }
   } 
}
