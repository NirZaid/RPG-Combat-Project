using GameDevTV.Utils;
using GameDevTV.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.UI.DamageText;
using UnityEngine.Events;

namespace RPG.Attributes
{
    using UnityEngine;

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70;
        [SerializeField] private TakeDamageEvent takeDamage;
        public UnityEvent onDie;

        private bool wasDeadLastFrame = false;
        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        public bool IsDead()
        {
            return healthPoints.value <= 0;
        }
        private LazyValue<float> healthPoints;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }


        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
           float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage / 100f;
           healthPoints.value = Mathf.Max(regenHealthPoints, healthPoints.value);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (IsDead())
            {
                onDie.Invoke();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
            
            UpdateState();
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if (!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }

            if (wasDeadLastFrame && !IsDead())
            {
                animator.Rebind();
            }


            wasDeadLastFrame = IsDead();
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;
            UpdateState();
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if(experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Heal(float amount)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + amount, GetMaxHealthPoints());
            UpdateState();
        }
    }
    

}
