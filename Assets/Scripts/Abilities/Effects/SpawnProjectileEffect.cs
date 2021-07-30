using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "SpawnProjectileEffect", menuName = "Abilities/Effects/Spawn Projectile", order = 0)]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] private Projectile projectileToSpawn;
        [SerializeField] private float damage;
        [SerializeField] private bool isRightHand = true;
        [SerializeField] private bool useTargetPoint = true;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            Fighter fighter = data.GetUser().GetComponent<Fighter>();
            Vector3 spawnPos = fighter.GetHandTransform(isRightHand).position;
            if (useTargetPoint)
            {
                SpawnProjectileForTargetPoint(data, spawnPos);
            }
            else
            {
                SpawnProjectilesForTargets(data, spawnPos);
            }

            finished();

        }

        private void SpawnProjectileForTargetPoint(AbilityData data, Vector3 spawnPos)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPos;
            projectile.SetTarget(data.GetTargetedPoint(), data.GetUser(), damage);
        }

        private void SpawnProjectilesForTargets(AbilityData data, Vector3 spawnPos)
        {
            foreach (GameObject target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPos;
                    projectile.SetTarget(health, data.GetUser(), damage);
                }
            }
        }
    }
}