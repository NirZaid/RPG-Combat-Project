using RPG.Core;
using UnityEngine;

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
        
        private float damage;
        private void Update()
        {
            if(target == null)
                return;
            if (isHoming && !target.isDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }
    
        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
    
        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
            
            Destroy(gameObject, maxLifeTime);
        }
    
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * (targetCollider.height) / 2;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target)
            {
                return;
            }
            if (target.isDead)
            {
                return;
            }
            target.TakeDamage(damage);
            projectileSpeed = 0;
    
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

