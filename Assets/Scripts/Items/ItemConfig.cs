using UnityEngine;
using UnityEngine.UI;

namespace Items
{
   [CreateAssetMenu(fileName = "ItemConfig", menuName = "ItemConfig/Make new Weapon", order = 0)]
   public class ItemConfig : ScriptableObject
   {
      public int damage = 0; 
      public int armour = 0;
      public bool canCutTrees = false; 
      public bool canBreakWalls = false;
      public bool canOpenDoors = false;
      public bool canBlockHits = false;
      
      [SerializeField] GameObject itemGameObject = null;
      [SerializeField] private Sprite itemSprite = null;

      [SerializeField]private int currentArmour = -1;

      /// <summary>
      ///  Sets slot image to this item's image
      /// </summary>
      /// <param name="image"></param>
      public void EquipNewItem(Image image)
      {
         if (itemGameObject != null)
         {
            itemSprite = itemGameObject.GetComponent<SpriteRenderer>().sprite;
            image.sprite = itemSprite;
         }
         else
         {
            image.sprite = itemSprite;
         }
      }

      public void DropThisItem(Vector3 pos)
      {
         GameObject item = Instantiate(itemGameObject, pos, Quaternion.identity);
         item.GetComponent<BoxCollider2D>().isTrigger = true;
      }

      public int GetCurrentArmout()
      {
         return currentArmour;
      }

      public void DecreaseArmour(int amount)
      {
         currentArmour -= amount;
      }

      public void ResetArmour()
      {
         currentArmour = armour;
      }
   } 
}
