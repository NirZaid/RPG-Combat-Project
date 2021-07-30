using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "OrientToTargetEffect", menuName = "Abilities/Effects/OrientToTarget", order = 0)]
    public class OrientToTargetEffect : EffectStrategy
    {

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.GetUser().transform.LookAt(data.GetTargetedPoint());
            finished();
        }
    }
}