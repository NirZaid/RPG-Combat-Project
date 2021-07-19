using UnityEngine;
using UnityEngine.UI;


namespace RPG.UI.DamageText
{

    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject damageTextPrefab;
        
        public void Spawn(float amount)
        {
            GameObject instance = Instantiate(damageTextPrefab, transform);
            instance.GetComponent<DamageText>().SetValue(amount);
        }
    }
}

