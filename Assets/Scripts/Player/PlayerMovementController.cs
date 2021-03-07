using System.Collections;
using Pathfinding;
using ProjectStavitski.Combat;
using ProjectStavitski.Core;
using ProjectStavitski.Items;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectStavitski.Player 
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Particles")] 
        [SerializeField] private GameObject hitParticle;
        [SerializeField] private GameObject pickupParticle;
        
        [SerializeField] private float speed = 10;
        [SerializeField] private LayerMask whatIsBlocked;
        [SerializeField] private LayerMask whatIsCollision;
        [SerializeField] private ItemConfig defaultItem;
        [SerializeField] private Image inventoryImage;
        
        private SingleNodeBlocker _blocker;
        private ItemConfig currentItem;


        private void Awake()
        {
            _blocker = GetComponent<SingleNodeBlocker>();
        }

        private void Start()
        {
            GameManager.Instance.player = gameObject;
            GameManager.Instance.AddObstacleToList(_blocker);

            _blocker.manager = GameManager.Instance.blockManager;
            GameManager.Instance.AddObstacleToList(_blocker);

            currentItem = defaultItem;
        }

        private void Update()
        {
            if (GameManager.Instance.playerTurn && !GameManager.Instance.PlayerMoving && !GameManager.Instance.UnitsMoving)
            {
                GetInput();
            }
        }

        private void GetInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y + 1);
                
                if (CheckForCollisions(transform.up)) return;
                
                StartCoroutine(SmoothMovement(destination,transform.up));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 destination = new Vector3(transform.position.x, transform.position.y - 1);
                
                if (CheckForCollisions(-transform.up)) return;
                
                StartCoroutine(SmoothMovement(destination,-transform.up));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                Vector3 destination = new Vector3(transform.position.x - 1, transform.position.y);
                
                if (CheckForCollisions(-transform.right)) return;
                
                StartCoroutine(SmoothMovement(destination,-transform.right));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Vector3 destination = new Vector3(transform.position.x + 1, transform.position.y);
                
                if (CheckForCollisions(transform.right)) return;
                
                StartCoroutine(SmoothMovement(destination,transform.right));
            }
        }

        private bool CheckForCollisions(Vector3 tr)
        {
            RaycastHit2D hitEnemy = Physics2D.Raycast(transform.position, tr,1.1f, whatIsCollision);

            if (hitEnemy.transform == null) return false;

            if (hitEnemy.transform.CompareTag("Obstacle")) return true;

            return CheckForItemInteractions(hitEnemy);
        }

        private bool CheckForItemInteractions(RaycastHit2D hitEnemy)
        {
            if (currentItem != defaultItem)
            {
                if (hitEnemy.transform.CompareTag("Tree") && currentItem.canCutTrees)
                { 
                    
                    Instantiate(hitParticle,hitEnemy.transform.position, Quaternion.identity);
                    Destroy(hitEnemy.transform.gameObject); 
                    return false;
                }
                
                if (hitEnemy.transform.CompareTag("Wall") && currentItem.canBreakWalls)
                { 
                    Instantiate(hitParticle,hitEnemy.transform.position, Quaternion.identity);
                    Destroy(hitEnemy.transform.gameObject); return false;
                }
                
                if (hitEnemy.transform.CompareTag("Door") && currentItem.canOpenDoors)
                {
                    Destroy(hitEnemy.transform.gameObject);
                    AstarPath.active.Scan();
            
                    currentItem = defaultItem;
                    EquipItem(defaultItem, transform.position);
                    return false;
                }
                            
                IDamageable enemy = hitEnemy.transform.gameObject.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    Instantiate(hitParticle, hitEnemy.transform.position, Quaternion.identity);
                    Attack(enemy);
                    return true;
                }

                // In case of other collisions
                if (hitEnemy.transform != null) return true;
            }
            
            if(hitEnemy.transform != null)
            {
                return true;
            }
            
            return false;
        }

        private void Attack(IDamageable enemy)
        {
            enemy.TakeDamage(currentItem.damage);
            GameManager.Instance.playerTurn = false;
        }

        
        /// <summary>
        /// Equips item and drops current at position
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pos"></param>
        public void EquipItem(ItemConfig item, Vector3 pos)
        {
            Instantiate(pickupParticle,pos,Quaternion.identity);
            
            if(currentItem != defaultItem)
                currentItem.DropThisItem(pos);
            
            currentItem = item;
            currentItem.EquipNewItem(inventoryImage);
        }

        public int GetCurrentItemArmour()
        {
            return currentItem.GetCurrentArmout();
        }

        public void DecreaseCurrentItemArmour(int amount)
        {
            currentItem.DecreaseArmour(amount);
            
            if (currentItem.GetCurrentArmout() <= 0 && currentItem.canBlockHits)
            {
                currentItem = defaultItem;
                EquipItem(currentItem,transform.position); 
            }
        }

        private IEnumerator SmoothMovement(Vector3 destination, Vector3 tr)
        {
            GameManager.Instance.PlayerMoving = true;
            
            RaycastHit2D hitBlock = Physics2D.Raycast(transform.position, tr,1.1f, whatIsBlocked);
            Debug.DrawRay(transform.position, tr, Color.red,3);
            if (hitBlock.transform == null)
            {
                while (transform.position != destination)
                {
                    transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                    GameManager.Instance.playerTurn = false;
                    yield return new WaitForEndOfFrame();
                }
            }

            GameManager.Instance.PlayerMoving = false;
        }
    }
}
