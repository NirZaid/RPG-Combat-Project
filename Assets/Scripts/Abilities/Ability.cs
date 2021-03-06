
using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "My Ability", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        [SerializeField] private TargetingStrategy targetingStrategy;
        [SerializeField] private FilterStrategy[] filterStrategies;
        [SerializeField] private EffectStrategy[] effectStrategies;
        [SerializeField] private float cooldownTime = 0f;
        [SerializeField] private float manaCost = 0;
        
        public override void Use(GameObject user)
        {
            Mana mana = user.GetComponent<Mana>();
            if (mana.GetMana() < manaCost)
            {
                return;
            }

            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0)
            {
                return;
            }

            AbilityData data = new AbilityData(user);
            
            user.GetComponent<ActionScheduler>().StartAction(data);
            
            targetingStrategy.StartTargeting(data,
                () =>
                {
                    TargetAquired(data);
                });
        }

        private void TargetAquired(AbilityData data)
        {
            if (data.IsCanceled())
            {
                return;
            }

            Mana mana = data.GetUser().GetComponent<Mana>();
            if (!mana.UseMana(manaCost))
            {
                return;
            }

            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);
            
            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }
          
        }

        private void EffectFinished()
        {
            
        }
    }
}

