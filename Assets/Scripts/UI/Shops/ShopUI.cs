using GameDevTV.Inventories;
using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
       

        [SerializeField] private TextMeshProUGUI shopName;
        [SerializeField] private Transform listRoot;
        [SerializeField] private RowUI rowPrefab;
        [SerializeField] private TextMeshProUGUI totalField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button switchButton;
        
        
        
        private Shopper shopper = null;
        private Shop currentShop = null;

        private Color originalTotalTextColor;
        
        private void Start()
        {
            originalTotalTextColor = totalField.color;
                shopper = GameObject.FindWithTag("Player").GetComponent<Shopper>();
            if (shopper == null)
            {
                return;
            }
            confirmButton.onClick.AddListener(ConfirmTransaction);
            switchButton.onClick.AddListener(SwitchMode);
            
            shopper.activeShopChange += ShopChanged;
            ShopChanged();
        }

        private void ShopChanged()
        {
            if (currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
            }

            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.SetShop(currentShop);
            }

            if (currentShop == null)
            {
                return;
            }
            shopName.text = currentShop.GetShopName();
            currentShop.onChange += RefreshUI;

            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate(rowPrefab, listRoot);
                row.Setup(currentShop,item );
            }
            
            totalField.text = $"Total: ${currentShop.TransactionTotal():N2}";
            totalField.color = currentShop.HasSufficientFunds() ? originalTotalTextColor : Color.red;
            confirmButton.interactable = currentShop.CanTransact();
            TextMeshProUGUI switchText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI confirmText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();
           
            if (currentShop.IsBuyingMode())
            { 
                switchText.text = "Switch To Selling"; 
                confirmText.text = "Buy";
            }
            else
            {
                switchText.text = "Switch To Buying";
                confirmText.text = "Sell";
            }
            
            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.RefreshUI();
            }

        }

        public void Close()
        {
            shopper.SetActiveShop(null);
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        public void SwitchMode()
        {
            currentShop.SelectMode(!currentShop.IsBuyingMode());
        }


    }

}
