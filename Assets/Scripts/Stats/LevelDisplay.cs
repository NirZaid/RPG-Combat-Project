using System;
using UnityEngine.UI;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private Text displayText;
        private BaseStats playerStats;

        private void Awake()
        {
            playerStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            displayText.text = playerStats.CalculateLevel().ToString();
        }
    }

}
