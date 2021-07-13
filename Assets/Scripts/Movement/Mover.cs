using RPG.Core;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private Transform target;

        private Health _health;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        // Update is called once per frame

        private void Start()
        {
            _health = GetComponent<Health>();
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            _navMeshAgent.enabled = !_health.isDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
          //  GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
        }

        public void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            _animator.SetFloat("forwardSpeed", speed);

        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }


    }
}

