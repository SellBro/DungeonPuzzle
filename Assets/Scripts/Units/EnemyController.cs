using System;
using System.Collections;
using Pathfinding;
using ProjectStavitski.Core;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectStavitski.Units
{
    public class EnemyController : MonoBehaviour, IComparable
    {
        public Transform playerPos;
        public bool useRaycastToAgro = true;

        [HideInInspector] public float distanceToPlayer;

        [SerializeField] private float speed = 5;
        [SerializeField] private float agroDistance = 8;
        [SerializeField] private LayerMask whatIsCollision;

        private EnemyUnit _unit;
        private SingleNodeBlocker _blocker;

        private EnemyState _state;
        private bool _shouldMove = false;
        private Vector3 _destination;
        private Vector3 _position;

        private bool followPlayer = false;

        private void Awake()
        {
            _unit = GetComponent<EnemyUnit>();
            _blocker = GetComponent<SingleNodeBlocker>();
        }

        private void Start()
        {
            _blocker.manager = GameManager.Instance.blockManager;
            playerPos = GameManager.Instance.player.transform;

            if (playerPos != null)
            {
                distanceToPlayer = Vector3.Distance(transform.position, playerPos.transform.position);
            }

            GameManager.Instance.AddUnitToList(this);
        }

        private void Update()
        {
            if (_shouldMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, _destination, speed * Time.deltaTime);
            }
        }

        public void Act()
        {
            distanceToPlayer = CalculateTargetDistance(transform.position);
            SetState();

            switch (_state)
            {
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Agro:
                    MoveTo(playerPos);
                    break;
                case EnemyState.Idle:
                    break;
            }
        }

        private void SetState()
        {
            if (distanceToPlayer < 1.2f)
            {
                _state = EnemyState.Attack;
            }
            else if (CheckForAgro() || followPlayer)
            {
                _state = EnemyState.Agro;
            }
            else
            {
                _state = EnemyState.Wander;
            }
        }

        private bool CheckForAgro()
        {
            if (useRaycastToAgro)
            {
                Debug.DrawRay(transform.position, transform.up, Color.green,2);
                Debug.DrawRay(transform.position, -transform.up, Color.green,2);
                Debug.DrawRay(transform.position, transform.right, Color.green, 2);
                Debug.DrawRay(transform.position, -transform.right, Color.green, 2);
                // Up
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, agroDistance, whatIsCollision);
                if (hit.transform != null && hit.transform.CompareTag("Player"))
                {
                    followPlayer = true;
                    return true;
                }
                    
                // Down 
                hit = Physics2D.Raycast(transform.position, -transform.up, agroDistance, whatIsCollision);
                if (hit.transform != null && hit.transform.CompareTag("Player"))
                { 
                    followPlayer = true;
                    return true; 
                }
                // Right 
                hit = Physics2D.Raycast(transform.position, transform.right, agroDistance, whatIsCollision);
                if (hit.transform != null && hit.transform.CompareTag("Player")) 
                { 
                    followPlayer = true;
                    return true; 
                }
                // Left
                hit = Physics2D.Raycast(transform.position, -transform.right, agroDistance, whatIsCollision);
                if (hit.transform != null && hit.transform.CompareTag("Player")) 
                {
                    followPlayer = true;
                    return true; 
                }
            }
            else
            {
                if (distanceToPlayer < agroDistance)
                {
                    return true;
                }
            }
            
            return false;
        }


        private void MoveTo(Transform target)
        {
            _blocker.Unblock();

            Path path = null;
            Vector3 destination = target.position;
            
            while (_unit.GetNumberOfMoves() > 0)
            {
                path = ConstructPath(destination);

                _unit.DecreaseMove();
                
                if (path != null)
                {
                    Debug.Log(_unit.GetTotalMoves() - _unit.GetNumberOfMoves());
                    Debug.Log(path.vectorPath[_unit.GetTotalMoves() - _unit.GetNumberOfMoves()]);
                    float disToPlayer = CalculateTargetDistance(path.vectorPath[_unit.GetTotalMoves() - _unit.GetNumberOfMoves()]);
                    if (disToPlayer < 1.1f)
                    {
                        Debug.Log("Player near");
                        break;
                    }
                    destination = transform.position + 2 * (path.vectorPath[1] - transform.position);
                }
            }
            
            if (path == null)
            {
                _blocker.BlockAt(_position); 
                return;
            }
            
            _position = path.vectorPath[_unit.GetTotalMoves() - _unit.GetNumberOfMoves()];
            _blocker.BlockAt(path.vectorPath[_unit.GetTotalMoves() - _unit.GetNumberOfMoves()]); 
            _destination = path.vectorPath[_unit.GetTotalMoves() - _unit.GetNumberOfMoves()];

            _unit.ResetMoves();
            
            StartCoroutine(SmoothMovement(_destination));

            distanceToPlayer = CalculateTargetDistance(transform.position);
        }

        private Path ConstructPath(Vector3 target)
        {
            var path = GameManager.Instance.ConstructPath(transform.position, target);
            
            if (path.error || path.vectorPath.Count <= 1) 
            { 
                return null;
            }

            return path;
        }
        
        private void Attack()
        {
            GameObject player = GameManager.Instance.player;
            player.GetComponent<Unit>().TakeDamage(_unit.GetDamage());
        }

        private IEnumerator SmoothMovement(Vector3 point)
        {
            _shouldMove = true;
            
            while (transform.position != point)
            {
                yield return new WaitForEndOfFrame();
            }
            
            _shouldMove = false;
        }

        private float CalculateTargetDistance(Vector3 pos)
        {
            return Vector3.Distance(pos, playerPos.position);
        }

        public void UnblockGridNode()
        {
            _blocker.Unblock();
        }

       public int CompareTo(object obj)
        {
            EnemyController enemy = (EnemyController) obj;
            if (distanceToPlayer > enemy.distanceToPlayer) return 1;
            else if (distanceToPlayer == enemy.distanceToPlayer) return 0;
            else return -1;
        }
    }

    public enum EnemyState
    {
        Wander,
        Idle,
        Agro,
        Attack
    }
}
