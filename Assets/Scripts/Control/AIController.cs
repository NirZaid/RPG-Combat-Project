using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float aggroTime = 5f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerence;
        [SerializeField] private float waypointDwellTime = 2f;
        [Range(0,1)] 
        [SerializeField] private float patrolSpeedFraction = 0.2f;

        [SerializeField] private float shoutDistance = 5f;

        private Health _health;
        private Fighter _fighter;
        private Mover _mover;
        private GameObject player;

        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceAggroed = Mathf.Infinity;
        LazyValue<Vector3> guardPosition;
        private int currentWaypointIndex = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }


        // Update is called once per frame
        void Update()
        {
            if(_health.isDead)
                return;
            if (IsAggrevated() && _fighter.CanAttack(player)) // Attacking 
            {
                AttackBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspicionTime) // Suspicion 
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }


        public void Agrrevate()
        {
            timeSinceAggroed = 0;
        }

        private void AggrevateNearbyEnemies()
        {
           RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
           foreach (RaycastHit hit in hits)
           {
               AIController ai = hit.collider.GetComponent<AIController>();
               if (ai == null)
               {
                   continue;
               }
               ai.Agrrevate();
           }
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggroed += Time.deltaTime;

        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            _fighter.Attack(player);
            
            AggrevateNearbyEnemies();
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            int waypointIndex = 0;
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerence;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggroed < aggroTime;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
        
        
}

       


