using System;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health Effect", order = 0)]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] private float healthChange = 20f;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health>();
                if (health)
                {
                    if (healthChange < 0)
                    {
                        health.TakeDamage(data.GetUser(), -healthChange);
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }
                }
               
            }

            finished();
        }
    }
}