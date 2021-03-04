using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using ProjectStavitski.Units;
using UnityEngine;

namespace ProjectStavitski.Core 
{
    public class GameManager : MonoBehaviour
    {
        [Header("Managing Components")]
        public static GameManager Instance = null;
        public BlockManager blockManager;
        
        [Header("Level settings")]
        public bool playerTurn = true;

        [Header("Player")]
        public GameObject player;
        
        [Header("Game Settings")]
        [SerializeField] private float turnDelay = 0.1f;
        
        private List<SingleNodeBlocker> _obstacles = new List<SingleNodeBlocker>();
        private List<EnemyController> _units = new List<EnemyController>();
        
        private BlockManager.TraversalProvider _traversalProvider;
        private bool _unitsMoving;
        private bool _playerMoving;

        private void Awake()
        {
            Instance = this;

            AstarPath.active = GetComponent<AstarPath>();
            blockManager = GetComponent<BlockManager>();
            _units = new List<EnemyController>();

            InitGame();
        }

        private void Start()
        {
            UpdateAStar();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) playerTurn = false;
            
            if (playerTurn || _unitsMoving || _playerMoving) return;

            StartCoroutine(MoveUnits());
        }

        private void InitGame()
        {
            _units.Clear();
        }

        public void UpdateAStar()
        {
            // Block Nodes under Units
            foreach (var unit in _units)
            {
                SingleNodeBlocker unitNode = unit.GetComponent<SingleNodeBlocker>();
                _obstacles.Add(unitNode);
            }
            
            // A* var
            _traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, _obstacles);
        }

        public void AddObstacleToList(SingleNodeBlocker obstacle)
        {
            _obstacles.Add(obstacle);
        }
        
        public void AddUnitToList(EnemyController unit)
        {
            _units.Add(unit);
        }
        
        public void RemoveUnitFromList(EnemyController unit)
        {
            _units.Remove(unit);
        }

        public bool PlayerMoving
        {
            get => _playerMoving;
            set => _playerMoving = value;
        }

        public bool UnitsMoving => _unitsMoving;

        private IEnumerator MoveUnits()
        {
            _unitsMoving = true;
            
            // Priority queue based on distance to player
            _units.Sort();
            
            foreach(EnemyController unit in _units)
            {
                unit.Act();
                yield return null;
            }

            yield return new WaitForSeconds(turnDelay);
            
            playerTurn = true;
            _unitsMoving = false;
        }
        
        
        public Path ConstructPath(Vector3 position, Vector3 target)
        {
            var path = ABPath.Construct(position, target, null);
            _traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.OnlySelector, _obstacles);
            
            // Make the path use a specific traversal provider
            path.traversalProvider = _traversalProvider;
            
            // Calculate the path synchronously
            AstarPath.StartPath(path);
            path.BlockUntilCalculated();
            
            if (path.error) 
            {
                Debug.Log("No path was found");
            } 

            return path;
        }
    }
}
