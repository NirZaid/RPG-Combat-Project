using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;



namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 6f;
        
        private Health _health;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        // Update is called once per frame

        private void Awake()
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

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
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


        public object CaptureState()
        {
            SerializableVector3 vector = new SerializableVector3(transform.position);
            return vector;
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;
            _navMeshAgent.enabled = false;
            transform.position = position.toVector();
            _navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

