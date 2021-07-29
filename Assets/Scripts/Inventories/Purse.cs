using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable
    {
        [SerializeField] private float startingBalance = 400f;
        public event Action onChange;
        private float balance = 0;

        private void Awake()
        {
            balance = startingBalance;
          
        }

        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;
            if (onChange != null)
            {
                onChange();
            }
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float) state;
        }
    }
 
}
