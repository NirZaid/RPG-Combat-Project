using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerence;
        [SerializeField] private float waypointDwellTime = 2f;

        private Health _health;
        private Fighter _fighter;
        private Mover _mover;
        private GameObject player;

        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private Vector3 guardPosition;
        private int currentWaypointIndex = 0;
        
        

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            guardPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(_health.isDead)
                return;
            if (InAttackRange() && _fighter.CanAttack(player)) // Attacking 
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

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            _fighter.Attack(player);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            int waypointIndex = 0;
            Vector3 nextPosition = guardPosition;

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
                _mover.StartMoveAction(nextPosition);
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

        private bool InAttackRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
        
        
}

       


