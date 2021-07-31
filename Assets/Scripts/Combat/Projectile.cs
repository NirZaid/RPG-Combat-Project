using RPG.Core;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Health target;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private bool isHoming;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit;
        [SerializeField] private float lifeAfterImpact = 0.2f;
        [SerializeField] private UnityEvent onHit;
        
        private float damage;
        private GameObject instigator;
        private Vector3 targetPoint;

        private void Update()
        {
            if (target != null && isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }
    
        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
    
        public void SetTarget(Health target,GameObject instigator, float damage)
        {
           SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;
            
            Destroy(gameObject, maxLifeTime);
        }
    
        private Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * (targetCollider.height) / 2;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponent<Health>();
            if (target != null && health != target)
            {
                return;
            }
            if (health == null || health.IsDead())
            {
                return;
            }

            if (other.gameObject == instigator)
            {
                return;
            }
            health.TakeDamage(instigator, damage);
            projectileSpeed = 0;
            onHit.Invoke();
    
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
            }
            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }

        
    }
}

