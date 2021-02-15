using System;
using System.Collections;
using Pathfinding;
using ProjectStavitski.Core;
using UnityEngine;

namespace ProjectStavitski.Units
{
    public class EnemyController : MonoBehaviour, IComparable
    {
        public Transform playerPos;
        public bool useRaycastToAgro = true;

        [HideInInspector] public float distanceToPlayer;

        [SerializeField] private float speed = 5;
        [SerializeField] private float agroDistance = 8;
        [SerializeField] private LayerMask whatIsPlayer;

        private EnemyUnit _unit;
        private SingleNodeBlocker _blocker;

        private EnemyState _state;
        private bool _shouldMove = false;
        private Vector3 _destination;
        private Vector3 _position;

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
            distanceToPlayer = CalculateTargetDistance();
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
            else if (CheckForAgro())
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
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, agroDistance, whatIsPlayer);
                if (hit.transform != null)
                    return true;
                // Right
                hit = Physics2D.Raycast(transform.position, -transform.up, agroDistance, whatIsPlayer);
                if (hit.transform != null)
                    return true;
                // Down
                hit = Physics2D.Raycast(transform.position, transform.right, agroDistance, whatIsPlayer);
                if (hit.transform != null)
                    return true;
                // Left
                hit = Physics2D.Raycast(transform.position, -transform.right, agroDistance, whatIsPlayer);
                if (hit.transform != null)
                    return true;
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
            var path = GameManager.Instance.ConstuctPath(transform, target);

            if (path.error || path.vectorPath.Count <= 1)
            {
                _blocker.BlockAt(_position);
                return;
            }

            _position = path.vectorPath[1];
            _blocker.BlockAt(path.vectorPath[1]); 
            _destination = path.vectorPath[1];

            StartCoroutine(SmoothMovement(_destination));
            
            distanceToPlayer = CalculateTargetDistance();
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

        private float CalculateTargetDistance()
        {
            return Vector3.Distance(transform.position, playerPos.position);
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
