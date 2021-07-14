using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

      

        public IEnumerator FadeOut(float time)
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}

