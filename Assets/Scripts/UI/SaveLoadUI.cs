using System;
using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [SerializeField] Transform contentRoot;
        [SerializeField] GameObject autoLayoutButtonPrefab;

        private void OnEnable()
        {
            
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper == null)
            {
                return;
            }
            foreach (Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }
            foreach (var save in savingWrapper.ListSaves())
            {
                GameObject buttonInstance = Instantiate(autoLayoutButtonPrefab, contentRoot);
                TMP_Text textComponent = buttonInstance.GetComponentInChildren<TMP_Text>();
                textComponent.text = save;
                Button button = buttonInstance.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    savingWrapper.LoadGame(save);
                });
            }
        }
    }

}
