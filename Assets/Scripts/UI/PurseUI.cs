using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI balanceField;

        private Purse playerPurse = null;

        private void Start()
        {
            playerPurse = GameObject.FindWithTag("Player").GetComponent<Purse>();
            if (playerPurse != null)
            {
                playerPurse.onChange += RefreshUI;
            }
            RefreshUI();
        }

        private void RefreshUI()
        {
            balanceField.text = $"${playerPurse.GetBalance():N2}";
        }
    }

}
