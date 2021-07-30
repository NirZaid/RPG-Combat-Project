using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class TraitRowUI : MonoBehaviour
    {
        [SerializeField] private Trait trait;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;
        private TraitStore playerTraitStore;

        public void Allocate(int points)
        {
            playerTraitStore.AssignPoints(trait,points);
        }

        private void Start()
        {
            minusButton.onClick.AddListener(() => Allocate(-1));
            plusButton.onClick.AddListener(() => Allocate(1));
            playerTraitStore = GameObject.FindWithTag("Player").GetComponent<TraitStore>();
        }

        private void Update()
        {
            minusButton.interactable = playerTraitStore.CanAssignPoints(trait,-1);
            plusButton.interactable = playerTraitStore.CanAssignPoints(trait,1);
            valueText.text = $"{playerTraitStore.GetProposedPoints(trait)}";
        }
    }
}
