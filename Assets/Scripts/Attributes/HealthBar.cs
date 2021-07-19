using System;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Canvas rootCanvas;

        private void Update()
        {
            float fraction = health.GetFraction();
            if (Mathf.Approximately(fraction, 0f) || Mathf.Approximately(fraction, 1f))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(fraction, 1, 1);
        }
        
    }

}
