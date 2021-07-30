using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        
        private LazyValue<float> mana;

        private void Awake()
        {
            mana = new LazyValue<float>(GetMaxMana);
        }

        public float GetMana()
        {
            return mana.value;
        }

        public float GetRegenRate()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ManaRegen);
        }

        public float GetMaxMana()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Mana);
        }

        public bool UseMana(float manaToUse)
        {
            if (manaToUse > mana.value)
            {
                return false;
            }

            mana.value -= manaToUse;
            return true;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (mana.value < GetMaxMana())
            {
                mana.value += GetRegenRate() * Time.deltaTime;
                if (mana.value > GetMaxMana())
                {
                    mana.value = GetMaxMana();
                }
            }
        }

        public object CaptureState()
        {
            return mana.value;
        }

        public void RestoreState(object state)
        {
            mana.value = (float) state;
        }
    }

}
