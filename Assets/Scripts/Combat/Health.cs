
namespace RPG.Combat
{
    using UnityEngine;

    public class Health : MonoBehaviour
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
            isDead = true;
        }
    }
    

}
