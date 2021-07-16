using RPG.Saving;

namespace RPG.Core
{
    using UnityEngine;

    public class Health : MonoBehaviour, ISaveable
    {
        public bool isDead { get; private set; }
        [SerializeField] private float healthPoints = 100f;

        public void TakeDamage(float damage)
        {
            if(isDead)
                return;
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            Debug.Log("Health: " + healthPoints);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if(isDead)
                return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float) state;
            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
    

}
