using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool wasActivated = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!wasActivated && other.gameObject.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                wasActivated = true;
            }
        }
    }
}

