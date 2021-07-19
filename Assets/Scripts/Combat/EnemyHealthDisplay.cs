using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter;
        [SerializeField] private Text healthText;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            Health health = fighter.GetTarget();
            if (health == null)
            {
                healthText.text = "";
            }
            else
            {
                healthText.text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            }
        }
    }
}

