using System;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Stats
{
    public class EXPDisplay : MonoBehaviour
    {
        [SerializeField] private Text displayText;
        private Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            displayText.text = String.Format("{0:0.0}", experience.GetPoints());
        }
    }

}
