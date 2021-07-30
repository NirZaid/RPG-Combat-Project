using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "DelayCompositeEffect", menuName = "Abilities/Effects/Delay Composite", order = 0)]
    public class DelayCompositeEffect : EffectStrategy
    {
        [SerializeField] private float delay = 0;
        [SerializeField] private EffectStrategy[] delayedEffects;
        [SerializeField] private bool abortIfCancelled = false;
        
        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(DelayedEffect(data, finished));
        }

        private IEnumerator DelayedEffect(AbilityData data, Action finished)
        {
            yield return new WaitForSeconds(delay);
            if (abortIfCancelled && data.IsCanceled())
            {
                yield break;
            }

            foreach (EffectStrategy effectStrategy in delayedEffects)
            {
                effectStrategy.StartEffect(data, finished);
            }
        }
    }
}